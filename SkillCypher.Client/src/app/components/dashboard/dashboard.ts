import { Component, OnInit } from '@angular/core';
import { Dashboard as DashboardService } from '../../services/dashboard';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule, RouterLink],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard implements OnInit {

  dashboardData: any = null;
  loading = false;
  error = '';

  constructor(
    private dashboardService: DashboardService,
    private cdr: ChangeDetectorRef

  ) { }

  ngOnInit(): void {
    this.loading = true;
    this.dashboardService.getDashBoard().subscribe({
      next: (data) => {
        this.dashboardData = data;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Dashboard request failed:', err);
        this.error = 'Unable to load your dashboard.';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }
}
