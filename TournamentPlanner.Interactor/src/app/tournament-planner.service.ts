import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { TP_BASE_URL } from './app.config';
import { Observable, of, retry } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TournamentPlannerService {

  constructor(private httpClient: HttpClient,@Inject(TP_BASE_URL) private baseUrl: string) { }


  public getTournament(name:string): Observable<any[]>{

    let params = new HttpParams();
    if(name){
      params = params.set("name", name);
    }
    return of([1,2,3]);
    // return this.httpClient.get<Observable<any>>(`${this.baseUrl}/tournament`,{params});
  }
}
