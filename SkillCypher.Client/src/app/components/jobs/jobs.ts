import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { Jobs as JobsService, Job } from '../../services/jobs';

@Component({
  selector: 'app-jobs',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './jobs.html',
  styleUrl: './jobs.css',
})
export class JobsComponent implements OnInit {
  jobs: Job[] = [];
  loading = false;
  errorMessage = '';
  search = '';
  location = '';
  page = 1;
  pageSize = 10;
  totalCount = 0;

  constructor(
    private jobsService: JobsService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.loadJobs();
  }

  get totalPages(): number {
    return Math.max(1, Math.ceil(this.totalCount / this.pageSize));
  }

  loadJobs(): void {
    this.loading = true;
    this.errorMessage = '';

    this.jobsService.getJobs(this.page, this.pageSize, this.search, this.location).subscribe({
      next: (response) => {
        this.jobs = response.jobs;
        this.totalCount = response.totalCount;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.jobs = [];
        this.totalCount = 0;
        this.loading = false;
        this.errorMessage = 'Unable to load jobs right now.';
        this.cdr.detectChanges();
      },
    });
  }

  applyFilters(): void {
    this.page = 1;
    this.loadJobs();
  }

  clearFilters(): void {
    this.search = '';
    this.location = '';
    this.page = 1;
    this.loadJobs();
  }

  goToPage(page: number): void {
    if (page < 1 || page > this.totalPages || page === this.page) {
      return;
    }

    this.page = page;
    this.loadJobs();
  }
}
