import { Component, Input } from '@angular/core';
import { ICourse } from '../../data/ICourse';

@Component({
  selector: 'app-course',
  standalone: true,
  imports: [],
  templateUrl: './course.component.html',
  styleUrl: './course.component.css'
})
export class CourseComponent {
  @Input() courses: ICourse[] = [];
}
