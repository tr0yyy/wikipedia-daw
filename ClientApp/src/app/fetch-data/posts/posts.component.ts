import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { EditorOption, EditorInstance } from 'angular-markdown-editor'
import { MarkdownService } from 'ngx-markdown'
import { environment } from 'src/environments/environment';

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
  
  markdownTextCopy = '';
  userType = 0;
  isEditing = false;
  rowsNo = 12;

  articol!: ArticolInterface;

  constructor(
    private fb: FormBuilder,
    private markdownService: MarkdownService,
    private http: HttpClient,
    private route: ActivatedRoute
  ) 
  { 
    http.get<ArticolInterface>(environment.apiPath + '/articol/' + this.route.snapshot.paramMap.get('title')).subscribe(async result => {
        
      this.articol = await result;
      console.log(this.articol);
      this.markdownText = this.articol.continut
    }, error => console.error(error));
  }

  ngOnInit() {
    this.editorOptions = {
      autofocus: false,
      iconlibrary: 'fa',
      savable: false,
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
});;
  }

  cancelEdit(event: Event) {
    this.markdownTextCopy = '';
    this.isEditing = false;
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