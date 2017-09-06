import { Injectable } from "@angular/core";
import {Http, RequestOptions, Headers, Response} from "@angular/http";
import {AppSettings} from "../app-settings";
import {User} from "../models/user";
import {Observable} from "rxjs/Observable";
import {TokenInfo} from "../models/token-info";

@Injectable()
export class AuthenticationService {
    private tokenInfoStorageKey = "currentTokenInfo";
    private currentUserStorageKey = "currentUser";

    constructor(private http: Http) {}

    login(username: string, password: string): Observable<User> {
        const headers = new Headers({ 'Content-Type': "application/json" });
        const options = new RequestOptions({ headers: headers });
        const reqBody: any = { username: username, password: password };
        return this.http
            .post(AppSettings.API_ENDPOINT + "/api/users/token", JSON.stringify(reqBody), options)
            .flatMap((response: Response) => {
                // login successful if there's a jwt token in the response
                const tokenInfo: TokenInfo = response.json();
                if (tokenInfo && tokenInfo.token) {
                    // store user details and jwt token in local storage to keep user logged in between page refreshes
                    localStorage.setItem(this.tokenInfoStorageKey, JSON.stringify(tokenInfo));
                    const currentUserHeaders = new Headers({
                        'Content-Type': "application/json",
                        'Authorization': `Bearer ${tokenInfo.token}`
                    });
                    const currentUserOptions = new RequestOptions({ headers: currentUserHeaders });
                    return this.http
                        .get(AppSettings.API_ENDPOINT + "/api/users/current", currentUserOptions)
                        .map((currentUserResponse: Response) => {
                            const currentUser: User = currentUserResponse.json();
                            localStorage.setItem(this.currentUserStorageKey, JSON.stringify(currentUser));
                            return currentUser;
                        });
                }
            });
    }

    register(username: string, password: string, confirmPassword: string, roleName: string): Observable<User> {
        const headers = new Headers({ 'Content-Type': "application/json" });
        const options = new RequestOptions({ headers: headers });
        const reqBody: any = {
            username: username,
            password: password,
            confirmPassword: confirmPassword,
            roleName: roleName
        };
        return this.http
            .post(AppSettings.API_ENDPOINT + "/api/users", JSON.stringify(reqBody), options)
            .map((response: Response) => response.json());
    }

    getLoggedInUser(): User {
        const currentUserStr = localStorage.getItem(this.currentUserStorageKey);
        return currentUserStr ? JSON.parse(currentUserStr) : null;
    }

    logout() {
        // remove user from local storage to log user out
        localStorage.removeItem(this.tokenInfoStorageKey);
        localStorage.removeItem(this.currentUserStorageKey);
    }
}
