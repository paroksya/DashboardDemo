import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})

export class LocalStorageService {

  intervalCount: number = 1000 * 60;
  storageList: any = [];

  public getLocalStorage(key: any) {
    return JSON.parse(sessionStorage.getItem(key) || '{}');
  }

  public setLocalStorageWithInterval(key: any, value: any) {
    this.setLocalStorage(key, value);
    if (this.storageList.indexOf(key) === -1) {
      this.storageList.push(key);
      this.timeInterval([key]);
    } else {
      clearTimeout(this.storageList[key]);
      this.timeInterval([key]);
    }
  }

  public setLocalStorage(key: any, value: any) {
    sessionStorage.setItem(key, JSON.stringify(value));
  }

  public removeStorage(keys: string[]) {
    keys.forEach(element => {
      sessionStorage.removeItem(element);
    });
  }

  public clearAllStorage() {
    sessionStorage.clear();
  }

  public timeInterval(key: any) {
    this.storageList[key] = setTimeout(() => {
      this.removeStorage(key);
      this.storageList.splice(this.storageList.indexOf(key), 1);
    }, this.intervalCount);
  }
}
