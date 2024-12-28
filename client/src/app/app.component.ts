import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './layout/header/header.component';
import { HttpClient } from '@angular/common/http';
import { Product } from './shared/models/Product';
import { Pagination } from './shared/models/Pagination';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent implements OnInit {
  baseUrl = 'https://localhost:5000/api/';
  private http = inject(HttpClient);
  products: Product[] = [];
  title = 'SkiNet';
  ngOnInit() {
    this.http.get<Pagination<Product>>(this.baseUrl + 'products').subscribe({
      next: (res) => {
        console.log(res.items);
        this.products = res.items;
      },
      error: (err) => console.log(err),
      complete: () => console.log('complete'),
    });
  }
}
