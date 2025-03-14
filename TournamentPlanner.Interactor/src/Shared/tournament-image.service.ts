import { Injectable } from '@angular/core';
import { GameTypeSupported } from '../app/tp-model/TpModel';

@Injectable({
  providedIn: 'root'
})
export class TournamentImageService {

  private defaultImage: string = "https://i.ibb.co.com/nNL7hwCh/DALL-E-2025-03-14-11-11-33-A-black-and-white-silhouette-style-vector-illustration-representing-a-gen.webp";
  private imageMap: Record<GameTypeSupported, string> = {
    [GameTypeSupported.TableTennis]: "https://i.ibb.co.com/j9C2373r/tt.jpg",
    [GameTypeSupported.EightBallPool]: "https://i.ibb.co.com/ksGdYgrX/eightballlogo.jpg"
  };

  constructor() { }

  public getTournamentCardImageUrl(gameType: GameTypeSupported | undefined | null | string): string {
    if (gameType == null || gameType == undefined) return this.defaultImage;

    return this.imageMap[gameType as GameTypeSupported] || this.defaultImage;
  }
}
