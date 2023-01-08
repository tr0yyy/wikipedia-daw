import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { EditorOption, EditorInstance } from 'angular-markdown-editor'
import { MarkdownService } from 'ngx-markdown'

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

  constructor(
    private fb: FormBuilder,
    private markdownService: MarkdownService
  ) { }

  ngOnInit() {
    this.editorOptions = {
      autofocus: false,
      iconlibrary: 'fa',
      savable: false,
      onShow: (e) => this.bsEditorInstance = e,
      parser: (val) => this.parse(val)
    };
    this.markdownText =
      `### Markdown example
---
This is an **example** where we bind a variable to the \`markdown\` component that is also bind to the editor.
#### example.component.ts
\`\`\`javascript
function hello() {
  alert('Hello World');
}
\`\`\`
#### example.component.html
\`\`\`html
<textarea [(ngModel)]="markdown"></textarea>
<markdown [data]="markdown"></markdown>
\`\`\``;

    this.buildForm(this.markdownText);
  }

  buildForm(markdownText: string) {
    this.templateForm = this.fb.group({
      body: [markdownText],
      isPreview: [true]
    });
  }

  editArticle(event: Event) {
    this.markdownTextCopy = this.markdownText;
    this.isEditing = true;
    this.markdownTextCopy = '**Alupigus**'
  }

  saveEdit(event: Event) {
    this.isEditing = false;
    this.markdownText = this.markdownTextCopy;
  }

  cancelEdit(event: Event) {
    this.markdownTextCopy = '';
    this.isEditing = false;
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
    this.highlight();

    return markedOutput;
  }
}
