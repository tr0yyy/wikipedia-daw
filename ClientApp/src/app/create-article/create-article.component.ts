import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule} from '@angular/forms';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import { ActivatedRoute, Router } from '@angular/router';
import { EditorOption, EditorInstance } from 'angular-markdown-editor'
import { MarkdownService } from 'ngx-markdown'
import { environment } from 'src/environments/environment';
import { AuthenticationService } from '../services/authentication.service';

@Component({
  templateUrl: './create-article.component.html',
  encapsulation: ViewEncapsulation.None,
  styleUrls: ['./create-article.component.css']
})

export class CreateArticleComponent implements OnInit {
  bsEditorInstance!: EditorInstance;
  markdownText = '';
  showEditor = true;
  templateForm!: FormGroup;
  editorOptions!: EditorOption;
  titleDomain!: FormGroup
  
  markdownTextCopy = '';
  userType = 0;
  isEditing = false;

  public isLoggedIn = this.authService.isLoggedIn()

  domeniuSelectat = 'Arta';
  

  isProtected = false;
  textProtected = "";
  isLocked = "";

  domenii: string[] = []



  articol = {} as ArticolInterface;

  constructor(
    private fb: FormBuilder,
    private markdownService: MarkdownService,
    private authService: AuthenticationService,
    private http: HttpClient,
    private router: Router
  ) 
  { 
    this.setTextIsProtected()
    this.http.get<string[]>(environment.apiPath + '/articol/alldomains').subscribe(async result => {
        this.domenii = result
    })
    
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
    this.titleDomain = this.fb.group({
      titlu: "",
      domeniu: ""
    })
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

  activateButton = false

  checkTitle(event : any) {
    if((event.target.value as string).replace(new RegExp(" ", 'g'), '') != "")
      this.activateButton = true
    else
      this.activateButton = false
  }

  saveEdit(event: Event) {
    this.markdownText = this.markdownTextCopy; // aici trebuie luat textul din form
    this.isEditing = false;
    this.articol.continut = this.markdownText;
    this.articol.protejat = this.isProtected;
    this.articol.titlu = this.titleDomain.get('titlu')?.value;
    this.articol.domeniu = this.domeniuSelectat

    console.log(this.articol);
    this.http.post<ArticolInterface>(
      environment.apiPath + '/articol/create', {
          Domeniu: this.articol.domeniu,
          Titlu: this.articol.titlu,
          User: this.authService.getUser()?.name,
          Continut: this.articol.continut,
          Protejat : this.articol.protejat,
      }
  ).subscribe({
    next: (result) => {
      console.log(result);
      this.router.navigate(['/articol/' + this.articol.titlu])
    },
    error: (error: HttpErrorResponse) => {
      console.log(error)
    }
});
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
  autor: string;
  data_adaugarii: string;
  continut: string;
  protejat: boolean;
  link: string;
}