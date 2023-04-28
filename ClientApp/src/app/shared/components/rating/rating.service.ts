import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Rating } from '../../models/rating';

@Injectable({
  providedIn: 'root'
})
export class RatingService {

  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  public rate(rating: Rating){
    console.log(rating)
    return this.http.post(this.baseUrl + 'rating', rating);
  }
}
