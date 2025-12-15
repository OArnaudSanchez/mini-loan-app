import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { LoanService } from './loans/services/loan.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent{

  private readonly loanService = inject(LoanService);

  loans = this.loanService.loans;

  displayedColumns: string[] = [
    'id',
    'amount',
    'currentBalance',
    'applicantName',
    'status',
  ];
  // loans = [
  //   {
  //     loanAmount: 25000.00,
  //     currentBalance: 18750.00,
  //     applicant: 'John Doe',
  //     status: 'active',
  //   },
  //   {
  //     loanAmount: 15000.00,
  //     currentBalance: 0,
  //     applicant: 'Jane Smith',
  //     status: 'paid',
  //   },
  //   {
  //     loanAmount: 50000.00,
  //     currentBalance: 32500.00,
  //     applicant: 'Robert Johnson',
  //     status: 'active',
  //   },
  // ];
}
