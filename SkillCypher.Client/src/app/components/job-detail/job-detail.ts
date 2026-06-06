import { CommonModule, DatePipe } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Auth } from '../../services/auth';
import { Jobs } from '../../services/jobs';
import { ApplicantProfileService } from '../../services/applicant-profile-service';
import { MatchService } from '../../services/match';


@Component({
  selector: 'app-job-detail',
  imports: [CommonModule, RouterLink, DatePipe],
  templateUrl: './job-detail.html',
  styleUrl: './job-detail.css',
})
export class JobDetail implements OnInit {
  job: any = null;
  matchScore: number | null = null;
  loading = false;
  errorMessage = '';
  constructor(
    private route: ActivatedRoute,
    private jobService: Jobs,
    private auth: Auth,
    private applicantProfileService: ApplicantProfileService,
    private matchService: MatchService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    const jobId = Number(idParam);

    if (!idParam || Number.isNaN(jobId)) {
      this.errorMessage = 'Invalid job id.';
      return;
    }
    this.loading = true;
    this.errorMessage = '';
    this.jobService.getJobById(jobId).subscribe({
      next: (job) => {
        this.job = job;
        this.loading = false;
        this.cdr.detectChanges();
        this.loadMatchScoreIfLoggedIn(jobId);
      },
      error: () => {
        this.loading = false;
        this.errorMessage = 'unable to load job details.';
        this.cdr.detectChanges();
      },
    });
  }

  private loadMatchScoreIfLoggedIn(jobId: number): void {
    if (!this.auth.isLoggedIn()) {
      return;
    }
    this.applicantProfileService.getProfile().subscribe({
      next: (profile) => {
        const applicantId = profile?.applicantId ?? profile?.ApplicantId;

        if (typeof applicantId !== 'number') {
          return;
        }

        this.matchService.getMatchScore(applicantId, jobId).subscribe({
          next: (result) => {
            this.matchScore = typeof result === 'number' ? result : result?.matchScore ?? result?.score ?? null;
            this.cdr.detectChanges();
          },
          error: () => {
            this.matchScore = null;
            this.cdr.detectChanges();
          },
        });
      },
      error: () => {
        this.matchScore = null;
        this.cdr.detectChanges();
      },
    });
  }
}
