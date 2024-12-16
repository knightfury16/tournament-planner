import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, signal } from '@angular/core';
import { TP_BASE_URL } from '../app/app.config';
import { firstValueFrom, Observable } from 'rxjs';
import { AddAdminDto, AddPlayerDto, LoginDto } from '../app/tp-model/TpModel';

export interface UserInfo {
  email: string;
  name: string;
  role: string;
}

@Injectable({
  providedIn: 'root'
})

export class AuthService {

  public currentUser = signal<UserInfo | null>(null);
  private accountBaseUrl: string;

  constructor(private httpClient: HttpClient, @Inject(TP_BASE_URL) private baseUrl: string) {
    this.accountBaseUrl = baseUrl + "/identity/account";
    this.initializeUserInfo();
  }

  public login(loginDto: LoginDto): Observable<UserInfo> {
    return this.httpClient.post<UserInfo>(`${this.accountBaseUrl}/login`, loginDto, { headers: { 'Content-Type': 'application/json' }, withCredentials: true });
  }

  public registerPlayer(addPlayerDto: AddPlayerDto): Observable<UserInfo> {
    return this.httpClient.post<UserInfo>(`${this.accountBaseUrl}/player`, { addPlayerDto });
  }

  public registerAdmin(addAdminDto: AddAdminDto): Observable<UserInfo> {
    return this.httpClient.post<UserInfo>(`${this.accountBaseUrl}/admin`, { addAdminDto });
  }

  public async singOut(): Promise<string> {
    var logoutResult = await firstValueFrom(this.httpClient.post<string>(`${this.accountBaseUrl}/logout`,null,{withCredentials: true}));
    console.log("LOGOUR:: ", logoutResult);
    this.currentUser.set(null);
    return logoutResult;
  }


  private async initializeUserInfo(): Promise<void> { 
    const user = await this.getUserInfo();
    if (user) { this.currentUser.set(user); } 
  }

  public async getUserInfo(): Promise<UserInfo | undefined> {
    return await firstValueFrom(this.httpClient.get<UserInfo | undefined>(`${this.accountBaseUrl}/userInfo`, {withCredentials: true}));
  }
}
