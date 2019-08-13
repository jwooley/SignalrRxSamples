import { Component, OnInit } from '@angular/core';
import { Observable, ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

let securityCache: ReplaySubject<Array<string>> = null;

@Injectable()
export class SecurityService {
  constructor(private http: HttpClient) { }


  public hasPermission$(permission: string): Observable<boolean> {
    if (!securityCache) {
      securityCache = new ReplaySubject(1);
      this.getPermissions$().subscribe(securityCache);
    }
    return securityCache.pipe(
      map(perm => perm.indexOf(permission) !== -1)
    );
  }

  public getPermissions$(): Observable<Array<string>> {
    return this.http.get<Array<string>>('/api/Security/UserPermissions');
  }
}
