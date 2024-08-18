import { Component, OnInit, Output, Input, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-button',
  standalone: true,
  imports: [],
  templateUrl: './button.component.html',
  styleUrl: './button.component.css'
})
export class ButtonComponent {
  @Input() text:string = "Press!";
  @Input() color:string = "blue";
  @Output() btnClick = new EventEmitter();

  constructor() { }

  ngOnInit(): void {

  }

  onClick(){
    this.btnClick.emit();
  }
}
