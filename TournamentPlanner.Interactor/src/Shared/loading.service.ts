import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  private isLoading = signal<boolean>(false);

  public show() {
    this.isLoading.set(true);
  }
  public hide() {
    this.isLoading.set(false);
  }
  public getIsLoading(): boolean {
    return this.isLoading()
  }
}
