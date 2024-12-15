import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, signal } from '@angular/core';
import { TP_BASE_URL } from '../app/app.config';
import { Observable } from 'rxjs';
import { AddAdminDto, AddPlayerDto, LoginDto } from '../app/tp-model/TpModel';

export interface UserInfo {
  email: string;
  name: string;
  role: string
}

@Injectable({
  providedIn: 'root'
})

export class AuthService {

  public currentUser = signal<UserInfo | null>(null);
  private accountBaseUrl: string;

  constructor(private httpClient: HttpClient, @Inject(TP_BASE_URL) private baseUrl: string) {
    this.accountBaseUrl = baseUrl + "/identity/account"
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

  public singOut(): void {
    this.currentUser.set(null);
  }
}
