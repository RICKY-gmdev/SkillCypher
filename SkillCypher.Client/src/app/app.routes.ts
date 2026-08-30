import { Routes } from '@angular/router';
import { Login } from './components/login/login';
import { JobsComponent } from './components/jobs/jobs';
import { Register } from './components/register/register';
import { JobDetail } from './components/job-detail/job-detail';
import { Onboarding } from './components/onboarding/onboarding';
import { Dashboard } from './components/dashboard/dashboard';

export const routes: Routes = [
    { path: '', redirectTo: 'login', pathMatch: 'full' },
    { path: 'login', component: Login },
    { path: 'jobs', component: JobsComponent },
    { path: 'register', component: Register },
    { path: 'jobs/:id', component: JobDetail },
    { path: 'onboarding', component: Onboarding },
    { path: 'dashboard', component: Dashboard }
];
