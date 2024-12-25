import { inject, Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class SnackbarService {

  //TODO:: Fix the Color Panel issue of warnign and error
  private _snackBar = inject(MatSnackBar);
  constructor() { }
  /**
   * Displays a message as a snackbar with a specified duration.
   * 
   * @param message The message to be displayed in the snackbar.
   * @param duration The duration in seconds for which the snackbar will be displayed. Defaults to 2 seconds.
   */
  showMessage(message: string, duration: number = 2): void {
    this._snackBar.open(message, 'OK', { horizontalPosition: 'center', verticalPosition: 'top', duration: duration * 1000 });
  }

  /**
   * Displays a warning message as a snackbar with a specified duration.
   * 
   * @param message The warning message to be displayed in the snackbar.
   * @param duration The duration in seconds for which the snackbar will be displayed. Defaults to 2 seconds.
   */
  showWarning(message: string, duration: number = 2): void {
    this._snackBar.open(message, 'OK', { horizontalPosition: 'center', verticalPosition: 'top', duration: duration * 1000, panelClass: ['warning-snackbar'] });
  }

  /**
   * Displays an error message as a snackbar with a specified duration.
   * 
   * @param message The error message to be displayed in the snackbar.
   * @param duration The duration in seconds for which the snackbar will be displayed. Defaults to 2 seconds.
   */
  showError(message: string, duration: number = 2): void {
    this._snackBar.open(message, 'OK', { horizontalPosition: 'center', verticalPosition: 'top', duration: duration * 1000, panelClass: ['error-snackbar'] });
  }
}
