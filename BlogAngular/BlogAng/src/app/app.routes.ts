import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AddBlogComponent } from './add-blog/add-blog.component';
import { BlogDetailComponent } from './blog-detail/blog-detail.component';
import { AuthGuard } from './auth.guard';
import { UserSettingsComponent } from './user-settings/user-settings.component';
import { UserPassComponent } from './user-pass/user-pass.component';
import { AdminLoginComponent } from './admin-login/admin-login.component';
import { AdminHomeComponent } from './admin-home/admin-home.component';
import { AdminAuthGuard } from './adminauth.guard';
import { AdminBlogComponent } from './admin-blog/admin-blog.component';
import { AdminCategoryComponent } from './admin-category/admin-category.component';
import { AdminSettingsComponent } from './admin-setttings/admin-setttings.component';
import { UserBlogsComponent } from './user-blogs/user-blogs.component';
import path from 'path';
import { BlogEditComponent } from './blog-edit/blog-edit.component';

export const routes: Routes = [
    {path:'', component:HomeComponent},
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    {path: 'addblog',component:AddBlogComponent,canActivate: [AuthGuard]},
    { path: 'changepassword', component: UserPassComponent,canActivate :[AuthGuard]},
    { path: 'usersettings', component: UserSettingsComponent,canActivate :[AuthGuard] },  
    {path : 'userBlog',component:UserBlogsComponent,canActivate:[AuthGuard]}, 
    {path: 'blogedit/:id',component:BlogEditComponent,canActivate:[AuthGuard]},
    { path: 'blog/:id', component: BlogDetailComponent },
    {path: 'addhome',component:AdminHomeComponent,canActivate:[AdminAuthGuard]},
    {path: 'addlog',component:AdminLoginComponent},
    {path: 'admin-blog',component:AdminBlogComponent,canActivate:[AdminAuthGuard]},
    {path: 'addcategory',component:AdminCategoryComponent,canActivate:[AdminAuthGuard]},
    {path: 'addSettings',component:AdminSettingsComponent,canActivate:[AdminAuthGuard]},
    { path: '**', redirectTo: '' } ,
    
];
