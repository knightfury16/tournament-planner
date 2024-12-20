import { HttpClient } from '@angular/common/http';
import { inject, Inject, Injectable, signal } from '@angular/core';
import { TP_BASE_URL } from '../app/app.config';
import { firstValueFrom, Observable } from 'rxjs';
import { AddAdminDto, AddPlayerDto, AdminDto, LoginDto, PlayerDto } from '../app/tp-model/TpModel';
import { LoadingService } from './loading.service';

export interface UserInfo {
  email: string;
  name: string;
  role: string;
}

@Injectable({
  providedIn: 'root'
})

export class AuthService {

  private currentUser = signal<UserInfo | null>(null);
  private accountBaseUrl: string;
  private loadingService = inject(LoadingService);

  constructor(private httpClient: HttpClient, @Inject(TP_BASE_URL) private baseUrl: string) {
    this.accountBaseUrl = baseUrl + "/identity/account";
    this.loadingService.show();
    this.initializeUserInfo().then(() => this.loadingService.hide());
  }

  public getCurrentUser() {
    return this.currentUser();
  }

  public setCurrentUser(userInfo: UserInfo | null) {
    this.currentUser.set(userInfo);
  }

  public login(loginDto: LoginDto): Observable<UserInfo> {
    return this.httpClient.post<UserInfo>(`${this.accountBaseUrl}/login`, loginDto, { headers: { 'Content-Type': 'application/json' }, withCredentials: true });
  }

  public registerPlayer(addPlayerDto: AddPlayerDto): Observable<PlayerDto> {
    return this.httpClient.post<PlayerDto>(`${this.baseUrl}/players`, addPlayerDto, { headers: { 'Content-Type': 'application/json' }, withCredentials: true });
  }

  public registerAdmin(addAdminDto: AddAdminDto): Observable<AdminDto> {
    return this.httpClient.post<AdminDto>(`${this.baseUrl}/admins`, addAdminDto, { headers: { 'Content-Type': 'application/json' }, withCredentials: true });
  }

  public async singOut(): Promise<string> {
    var logoutResult = await firstValueFrom(this.httpClient.post<string>(`${this.accountBaseUrl}/logout`, null, { withCredentials: true }));
    console.log("LOGOUR:: ", logoutResult);
    this.currentUser.set(null);
    return logoutResult;
  }


  //making it pulblic so that i can initialize it before reading current user in authGuard, adminGuard
  public async initializeUserInfo(): Promise<void> {
    if (this.currentUser()) return;
    const user = await this.getUserInfo();
    if (user) { this.currentUser.set(user); }
  }

  public async getUserInfo(): Promise<UserInfo | undefined> {
    return await firstValueFrom(this.httpClient.get<UserInfo | undefined>(`${this.accountBaseUrl}/userInfo`, { withCredentials: true }));
  }
}
