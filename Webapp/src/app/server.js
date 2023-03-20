const cors = require('cors');
const http = require('http');
const port = 5286;
const server = http.createServer((req, res) => {
  res.setHeader('Access-Control-Allow-Origin', '*');
  res.setHeader('Access-Control-Allow-Headers', 'Origin, X-Requested-With, Content, Accept, Content-Type, Authorization,X-Auth-Token');
  res.setHeader('Access-Control-Allow-Methods', 'GET', 'POST', 'PUT', 'DELETE');
  res.write('test');
  res.end();
  app.use(cors());
});
server.listen(port, () => {
  console.log('listening on port ${port}.....');
})

