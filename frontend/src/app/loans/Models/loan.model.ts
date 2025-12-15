import { LoanStatus } from "../enums/loan-status.enum";

export interface Loan{
    id: string;
    amount: number;
    currentBalance: number;
    applicantName: string;
    status: LoanStatus; 
}