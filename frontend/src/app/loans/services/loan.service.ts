import { inject, Injectable, signal } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Loan } from "../Models/loan.model";
import { LoanResponse } from "../Models/loan-response.model";
import { map, tap } from "rxjs";
import { LoanMapper } from "../../mappers/loan.mapper";


@Injectable({
    providedIn: 'root'
})
export class LoanService{
    
    readonly BASE_URL = 'http://localhost:8080';
    private readonly httpClient = inject(HttpClient);

    readonly loans = signal<Loan[]>([]);

    constructor(){
        this.getLoans();
    }

    getLoans(){
        this.httpClient.get<LoanResponse[]>(`${this.BASE_URL}/loan`)
        .pipe(
            map((loans) => LoanMapper.mapLoans(loans)),
            tap(loans => this.loans.set(loans)),
            //TODO: Handle api errors
        )
        .subscribe();
    }
}