import { inject, Inject, Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { TP_BASE_URL } from '../app/app.config';
import { HttpClient, HttpParams } from '@angular/common/http';
import { TournamentDto } from '../app/tp-model/TpModel';

@Injectable({
  providedIn: 'root'
})
export class AdminTournamentService {

  private baseUrl: string = inject(TP_BASE_URL);
  private httpClient: HttpClient = inject(HttpClient);

  public getAdminTournaments(): Promise<TournamentDto[]> {
    return firstValueFrom(this.httpClient.get<TournamentDto[]>(`${this.baseUrl}/tournament/created/admin`, { withCredentials: true }));
  }
}
