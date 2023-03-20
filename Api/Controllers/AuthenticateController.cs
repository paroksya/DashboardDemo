using DemoDynamicRolePolicy.AuthPolicy;
using DemoDynamicRolePolicy.DBContext;
using DemoDynamicRolePolicy.IdentityAuth;
using DemoDynamicRolePolicy.Interface;
using DemoDynamicRolePolicy.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace DemoDynamicRolePolicy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        public string PolicyName { get; set; }

        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly IRoleRepository _roleRepository;
        private readonly ApplicationDbContext _context;

        public AuthenticateController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IRoleRepository roleRepository, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            _roleRepository = roleRepository;
            _context = context;
        }

        [HttpPost]
        [Route("RegisterAdmin")]
        public async Task<APIResponse> RegisterAdmin([FromBody] ApplicationUser model)
        {
            try
            {
                var userExists = await userManager.FindByNameAsync(model.UserName);
                if (userExists != null)
                    return new APIResponse { Success = false, Message = "User Already Exists!", Data = null };

                ApplicationUser user = new ApplicationUser()
                {
                    Email = model.UserName,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    roleName = model.roleName,
                    IsActive = true
                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                    return new APIResponse { Success = false, Message = "User Creation Failed!Please try Again.", Data = null };

                var userRoles = await userManager.GetRolesAsync(user);

                var rolePolicy = _roleRepository.GetRolesBasedPolicies();

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(ClaimTypes.Role,user.roleName),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authsigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

                var token = new JwtSecurityToken
                            (
                                issuer: _configuration["JWT:ValidIssuer"],
                                audience: _configuration["JWT:ValidAudience"],
                                expires: DateTime.Now.AddHours(3),
                                claims: authClaims
                            );

                dynamic data = new ExpandoObject();
                data.token = new JwtSecurityTokenHandler().WriteToken(token);
                data.expiration = token.ValidTo;
                data.user = user;
                return new APIResponse { Success = true, Message = "User created successfully!", Data = data };
            }
            catch (Exception ex)
            {
                return new APIResponse { Success = false, Message = "Error : " + ex.Message, Data = null };
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<APIResponse> Login([FromBody] LoginModel model)
        {
            var response = new APIResponse();
            try
            {
                var user = await userManager.FindByNameAsync(model.UserName);
                if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
                {
                    if (user.IsActive)
                    {
                        if (model.roleName == "admin" && user.roleName != "admin")
                        {
                            response.Success = false;
                            response.Message = "User not exist";
                            response.Data = null;
                            return response;
                        }

                        if (model.roleName == "User" && user.roleName != "User")
                        {
                            response.Success = false;
                            response.Message = "User not exist";
                            response.Data = null;
                            return response;
                        }

                        var userRoles = await userManager.GetRolesAsync(user);

                        var permissionPolicy = _roleRepository.GetRolesBasedPolicies();

                        var policy = new List<Claim>();

                        var authClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name,user.UserName),
                            new Claim(ClaimTypes.Role,user.roleName),
                            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                        };

                        foreach (var item in permissionPolicy)
                        {
                            policy.Add(new Claim(CustomClaimPolicy.policyName, item.policyName, item.roleName));
                        }

                        var authsigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

                        var token = new JwtSecurityToken(
                            issuer: _configuration["Issuer"],
                            audience: _configuration["Audience"],
                            expires: DateTime.Now.AddHours(3),
                            claims: authClaims,
                            signingCredentials: new SigningCredentials(authsigninKey, SecurityAlgorithms.HmacSha256)
                            );

                        dynamic data = new ExpandoObject();
                        data.token = new JwtSecurityTokenHandler().WriteToken(token);
                        data.expiration = token.ValidTo;
                        data.user = user;
                        data.permission = policy;
                        response.Success = true;
                        response.Message = "Login Successfully";
                        response.Data = data;
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "User is not active. Please contact to administrator";
                        response.Data = null;
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "User Not Found or Wrong Credentials";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error:" + ex.Message;
                response.Data = null;
            }
            return response;
        }


        [HttpPost]
        [Route("SocialRegister")]
        public async Task<APIResponse> SocialRegister([FromBody] ApplicationUser model)
        {
            try
            {
                var userExists = await userManager.FindByEmailAsync(model.Email);
                ApplicationUser user = new ApplicationUser()
                {
                    Email = model.UserName,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    roleName = model.roleName,
                    IsActive = true,
                    PhoneNumber = model.PhoneNumber,
                    provider = model.provider,
                    photoUrl = model.photoUrl
                };
                if (userExists != null)
                {
                    user = userExists;
                    if (userExists.IsActive)
                    {
                        userExists.FirstName = model.FirstName;
                        userExists.LastName = model.LastName;
                        userExists.provider = model.provider;
                        userExists.photoUrl = model.photoUrl;
                        userExists.IsActive = true;
                        var result = await userManager.UpdateAsync(userExists);
                        if (!result.Succeeded)
                        {
                            var errormsg = string.Join(' ', result.Errors.Select(z => z.Description).ToList());
                            return new APIResponse { Success = false, Message = errormsg, Data = null };
                        }
                    }
                    else
                    {
                        return new APIResponse { Success = false, Message = "User is not active. Please contact to administrator", Data = null };
                    }
                }
                else
                {
                    var result = await userManager.CreateAsync(user);
                    if (!result.Succeeded)
                    {
                        var errormsg = string.Join(' ', result.Errors.Select(z => z.Description).ToList());
                        return new APIResponse { Success = false, Message = errormsg, Data = null };
                    }
                    if (!await roleManager.RoleExistsAsync(user.roleName))
                        await roleManager.CreateAsync(new IdentityRole(user.roleName));
                    if (await roleManager.RoleExistsAsync(user.roleName))
                    {
                        await userManager.AddToRoleAsync(user, user.roleName);
                    }
                }
                var userRoles = await userManager.GetRolesAsync(user);

                var permissionPolicy = _roleRepository.GetRolesBasedPolicies();

                var policy = new List<Claim>();

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role,user.roleName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var item in permissionPolicy)
                {
                    policy.Add(new Claim(CustomClaimPolicy.policyName, item.policyName, item.roleName));
                }
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
                var token = new JwtSecurityToken
                            (
                                issuer: _configuration["JWT:ValidIssuer"],
                                audience: _configuration["JWT:ValidAudience"],
                                expires: DateTime.Now.AddHours(24),
                                claims: authClaims,
                                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                            );

                dynamic data = new ExpandoObject();
                data.token = new JwtSecurityTokenHandler().WriteToken(token);
                data.expiration = token.ValidTo;
                data.user = userExists != null ? userExists : user;
                data.permission = policy;
                return new APIResponse { Success = true, Message = (user == null) ? "User created successfully!" : "User Updated successfully!", Data = data };
            }
            catch (Exception ex)
            {
                return new APIResponse { Success = false, Message = "Error : " + ex.Message, Data = null };
            }
        }
    }
}
