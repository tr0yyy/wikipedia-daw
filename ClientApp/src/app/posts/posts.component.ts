import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule} from '@angular/forms';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import { ActivatedRoute, Router } from '@angular/router';
import { EditorOption, EditorInstance } from 'angular-markdown-editor'
import { MarkdownService } from 'ngx-markdown'
import { environment } from 'src/environments/environment';
import { RoleEnum } from '../models/role.enum';
import { AuthenticationService } from '../services/authentication.service';


@Component({
  templateUrl: './posts.component.html',
  encapsulation: ViewEncapsulation.None,
  styleUrls: ['./posts.component.css']
})

export class PostsComponent implements OnInit {
  bsEditorInstance!: EditorInstance;
  markdownText = '';
  showEditor = true;
  templateForm!: FormGroup;
  editorOptions!: EditorOption;

  public canChangeProtejat = false;
  public isModerator = false;
  public canBeEdited = false;
  
  markdownTextCopy = '';
  userType = 0;
  isEditing = false;

  isProtected = false;
  textProtected = "";
  isLocked = "";

  articol!: ArticolInterface;

  constructor(
    private fb: FormBuilder,
    private markdownService: MarkdownService,
    private http: HttpClient,
    private route: ActivatedRoute,
    private authService: AuthenticationService,
    private router: Router,
  ) 
  { console.log(this.authService.getRoles())
    http.get<ArticolInterface>(environment.apiPath + '/articol/' + this.route.snapshot.paramMap.get('title')).subscribe(async result => {
        
      this.articol = await result;
      console.log(this.articol);
      console.log(this.authService.getUser()?.name == this.articol.user)
      this.canChangeProtejat = (this.authService.getRoles()?.includes(RoleEnum.Moderator) 
      || this.authService.getUser()?.name == this.articol.user) ? true : false;
      this.isModerator = this.authService.getRoles()?.includes(RoleEnum.Moderator) ? true : false;
      this.markdownText = this.articol.continut
      this.isProtected = this.articol.protejat
      this.canBeEdited = this.authService.getUser() || !this.isProtected ? true : false
      this.setTextIsProtected()
    }, error => console.error(error));
  }

  ngOnInit() {
    this.editorOptions = {
      autofocus: false,
      iconlibrary: 'fa',
      savable: false,
      resize:"vertically",
      onShow: (e) => this.bsEditorInstance = e,
      parser: (val) => this.parse(val)
    };
    

    this.buildForm(this.markdownText);
    this.onFormChanges();
  }

  buildForm(markdownText: string) {
    this.templateForm = this.fb.group({
      body: [markdownText],
    });
  }

  editArticle(event: Event) {
    this.isEditing = true;
    this.markdownTextCopy = this.markdownText;
  }

  saveEdit(event: Event) {
    this.markdownText = this.markdownTextCopy; // aici trebuie luat textul din form
    this.isEditing = false;
    this.articol.continut = this.markdownText;
    this.articol.protejat = this.isProtected;

    console.log(this.articol);
    this.http.post<ArticolInterface>(
      environment.apiPath + '/articol/update-articol', {
          Titlu: this.articol.titlu,
          Continut: this.articol.continut,
          Protejat : this.articol.protejat,
      }
  ).subscribe({
    next: (result) => {
      console.log(result);
    },
    error: (error: HttpErrorResponse) => {
      console.log(error)
    }
});
  }

  revertEdit(event: Event) {
    console.log(this.articol.titlu);
    this.http.post<ArticolInterface>(
      environment.apiPath + '/articol/revert-articol', {
          Titlu: this.articol.titlu,
          Continut: this.articol.continut,
          Protejat : this.articol.protejat,
      }
  ).subscribe({
    next: (result) => {
      console.log(result);
      window.location.reload()
    },
    error: (error: HttpErrorResponse) => {
      console.log(error)
    }
});
  }

  cancelEdit(event: Event) {
    this.markdownTextCopy = '';
    this.isEditing = false;

    // Dupa ce este luata valoarea de protectie a articolului, trebuie resetat slide-ul de protect
  }

  onFormChanges(): void {
    this.templateForm.valueChanges.subscribe(formData => {
      if (formData) {
        this.markdownTextCopy = formData.body;
      }
    });
  }

  highlight() {
    setTimeout(() => {
      this.markdownService.highlight();
    });
  }

  hidePreview() {
    if (this.bsEditorInstance && this.bsEditorInstance.hidePreview) {
      this.bsEditorInstance.hidePreview();
    }
  }


  setTextIsProtected(){
    if (this.isProtected){
      this.textProtected = "Articol protejat  ";
      this.isLocked = "locked";
    }
    else {
      this.textProtected = "Articol neprotejat";
      this.isLocked = "lock_open";
    }
  }

  checkIsProtected($event: MatSlideToggleChange){
    this.isProtected = $event.checked
    this.setTextIsProtected();
    
  }

  showFullScreen(isFullScreen: boolean) {
    if (this.bsEditorInstance && this.bsEditorInstance.setFullscreen) {
      this.bsEditorInstance.showPreview();
      this.bsEditorInstance.setFullscreen(isFullScreen);
    }
  }

  parse(inputValue: string) {
    const markedOutput = this.markdownService.parse(inputValue.trim());

    return markedOutput;
  }
}

interface ArticolInterface {
  Id: number;
  domeniu: string;
  titlu: string;
  user: string;
  data_adaugarii: string;
  continut: string;
  protejat: boolean;
  link: string;
  token?: string
}