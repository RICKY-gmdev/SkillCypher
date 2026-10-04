import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { RecruiterDashboard as RecruiterDashboardService } from '../../services/recruiter-dashboard';

@Component({
  selector: 'app-recruiter-dashboard',
  imports: [],
  templateUrl: './recruiter-dashboard.html',
  styleUrl: './recruiter-dashboard.css',
})
export class RecruiterDashboard implements OnInit {
  dashboardData: any = null;
  loading = false;
  error = '';
constructor(
  private recruiterDashboardService: RecruiterDashboardService,
  private cdr: ChangeDetectorRef
) { }

ngOnInit(): void {
  this.loading = true;
  this.recruiterDashboardService.getRecruiterDashboard().subscribe({
    next: (data) => {
      this.dashboardData = data;
      this.loading = false;
      this.cdr.detectChanges();
    },
    error: (err) =>{
      console.error('Dashboard request failse:',err);
      this.error = 'Unable to load your dashboard.';
      this.loading = false;
      this.cdr.detectChanges();
    }
  });
}
  
}
