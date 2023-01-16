import { Component, OnInit, NgZone } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CredentialResponse, PromptMomentNotification } from 'google-one-tap';
import { environment } from 'src/environments/environment';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  private clientId = environment.clientId

  constructor(
    private router: Router,
    private service: AuthService,
    private _ngZone: NgZone) { }

    ngOnInit(): void {
      // @ts-ignore
      window.onGoogleLibraryLoad = () => {
        // @ts-ignore
        google.accounts.id.initialize({
          client_id: this.clientId,
          callback: this.handleCredentialResponse.bind(this),
          auto_select: false,
          cancel_on_tap_outside: true
        });
        // @ts-ignore
        google.accounts.id.renderButton(
        // @ts-ignore
        document.getElementById("buttonDiv"),
          { theme: "outline", size: "large", width: "100%" } 
        );
        // @ts-ignore
        google.accounts.id.prompt((notification: PromptMomentNotification) => {});
      };
    }

    async handleCredentialResponse(response: CredentialResponse) {
      debugger;
      await this.service.LoginWithGoogle(response.credential).subscribe(
        (x:any) => {
          debugger;
          localStorage.setItem("token", x.token);
          this._ngZone.run(() => {
            this.router.navigate(['/logout']);
          })},
        (error:any) => {
            console.log(error);
          }
        );  
  }
}