import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';
import { ApplicantProfileService } from '../../services/applicant-profile-service';
import { Skill, SkillsService } from '../../services/skills';

@Component({
  selector: 'app-onboarding',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './onboarding.html',
  styleUrl: './onboarding.css',
})
export class Onboarding implements OnInit {
  step = 1;
  onboardingPath: 'resume' | 'manual' | null = null;
  searchTerm = '';
  skills: Skill[] = [];
  selectedSkillIds = new Set<number>();
  loadingSkills = false;
  saving = false;
  errorMessage = '';

  constructor(
    private skillsService: SkillsService,
    private applicantProfileService: ApplicantProfileService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadSkills();
  }

  get filteredSkills(): Skill[] {
    const query = this.searchTerm.trim().toLowerCase();

    if (!query) {
      return this.skills;
    }

    return this.skills.filter((skill) =>
      skill.skillName.toLowerCase().includes(query)
    );
  }

  get selectedSkills(): Skill[] {
    return this.skills.filter((skill) => this.selectedSkillIds.has(skill.skillId));
  }

  selectPath(path: 'resume' | 'manual'): void {
    this.onboardingPath = path;
    this.errorMessage = '';
  }

  nextStep(): void {
    this.errorMessage = '';

    if (this.step === 1 && !this.onboardingPath) {
      this.errorMessage = 'Please choose how you want to continue.';
      return;
    }

    if (this.step === 2 && this.onboardingPath === 'manual' && this.selectedSkillIds.size === 0) {
      this.errorMessage = 'Please select at least one skill before continuing.';
      return;
    }

    this.step = Math.min(3, this.step + 1);
  }

  previousStep(): void {
    this.errorMessage = '';
    this.step = Math.max(1, this.step - 1);
  }

  toggleSkill(skillId: number): void {
    if (this.selectedSkillIds.has(skillId)) {
      this.selectedSkillIds.delete(skillId);
    } else {
      this.selectedSkillIds.add(skillId);
    }

    this.selectedSkillIds = new Set(this.selectedSkillIds);
  }

  removeSkill(skillId: number): void {
    if (!this.selectedSkillIds.has(skillId)) {
      return;
    }

    this.selectedSkillIds.delete(skillId);
    this.selectedSkillIds = new Set(this.selectedSkillIds);
  }

  saveAndContinue(): void {
    this.errorMessage = '';

    if (this.onboardingPath !== 'manual' || this.selectedSkillIds.size === 0) {
      this.router.navigate(['/jobs']);
      return;
    }

    this.saving = true;

    const requests = this.selectedSkills.map((skill) =>
      this.applicantProfileService.addSkill(skill.skillId)
    );

    forkJoin(requests).subscribe({
    next: () => {
        this.applicantProfileService.syncSkills().subscribe({
            next: () => this.router.navigate(['/jobs']),
            error: () => this.router.navigate(['/jobs']) // navigate anyway
        });
    }
  });
  }

  private loadSkills(): void {
    this.loadingSkills = true;

    this.skillsService.getSkills().subscribe({
      next: (skills) => {
        this.skills = skills;
        this.loadingSkills = false;
      },
      error: () => {
        this.loadingSkills = false;
        this.errorMessage = 'Unable to load skills at the moment.';
      },
    });
  }
}
