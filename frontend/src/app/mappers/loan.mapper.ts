import { LoanStatus } from "../loans/enums/loan-status.enum";
import { LoanResponse } from "../loans/Models/loan-response.model";
import { Loan } from "../loans/Models/loan.model";

export class LoanMapper{
    static mapLoan(loan: LoanResponse): Loan{
        const { id, applicantName, amount, currentBalance, status  } = loan;
        return {
            id,
            applicantName,
            amount,
            currentBalance,
            status: status == 1 ? 'Active' : 'Paid'
        }
    }

    static mapLoans(loans: LoanResponse[]): Loan[]{
        return loans.map(loan => this.mapLoan(loan));
    }
}