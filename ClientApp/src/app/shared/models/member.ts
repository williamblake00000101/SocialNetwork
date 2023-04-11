import { Photo } from "./photo";

export interface Member {
  id: number;
  userName: string;
  firstName: string;
  lastName: string
  photoUrl: string;
  age: number;
  created: Date;
  lastActive: Date;
  gender: string;
  introduction: string;
  lookingFor: string;
  interests: string;
  city: string;
  country: string;
  specialization: string;
  photos: Photo[];
}
