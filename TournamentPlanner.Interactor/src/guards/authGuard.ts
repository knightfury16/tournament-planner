import { inject } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivateFn, Router, RouterStateSnapshot } from "@angular/router";
import { AuthService } from "../Shared/auth.service";

export const authGuard: CanActivateFn = async (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
    const authService = inject(AuthService);
    const router = inject(Router);

    await authService.initializeUserInfo();
    var currentUser = authService.getCurrentUser();

    if (currentUser) {
        return true;
    }
    else {
        router.navigate(['/login']);
        return false;
    }
}