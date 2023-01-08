import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { Result } from "../services/result.interface";

@Injectable({
    providedIn: 'root'
})

export class AccountClient {
    constructor(private http: HttpClient) {
    }

    public login(userName: string, password: string): Observable<Result<string>> {
        return this.http.post<Result<string>>(
            environment.apiPath + '/accounts/login', {
                username: userName,
                password: password
            }
        );
    };

    public register(email: string, userName: string, password: string): Observable<Result<string>> {
        return this.http.post<Result<string>>(
            environment.apiPath + '/accounts/register', {
                email: email,
                username: userName,
                password: password
            }
        );
    };

}
