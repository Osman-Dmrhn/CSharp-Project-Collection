import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';  
import { HomeComponent } from './home/home.component';  
import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule,HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './services/auth.interceptor'; 
import { AddBlogComponent } from './add-blog/add-blog.component';
import { BlogService } from './services/blog.service';

@NgModule({
  declarations: [
    AppComponent,  
    HomeComponent,
    AddBlogComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    HttpClient,  
    CommonModule,
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
  },
  BlogService,
],
  bootstrap: [AppComponent] 
})
export class AppModule { }
