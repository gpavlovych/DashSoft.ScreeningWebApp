import { Injectable } from "@angular/core";
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs/Observable";
import {NavbarService} from "../services/navbar.service";

@Injectable()
export class PublicNoNavbarGuard implements CanActivate {
    constructor(private navbarService: NavbarService) {
    }

    canActivate(
        next: ActivatedRouteSnapshot,
        state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
        this.navbarService.hide();
        return true;
    }
}
