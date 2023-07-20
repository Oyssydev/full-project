import { Injectable } from '@angular/core';
import { Etudiant } from '../models/etudiants';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environment/environment';


@Injectable({
  providedIn: 'root'
})
export class EtudiantService {
  private url ="etudiants";
  private exportURL_XLSX ="export/xlsx";
  private exportURL_PDF ="export/pdf";

  alertModuleOIsOn = false;
  constructor(private http : HttpClient) { }
  

  //makes an HTTP GET request to retrieve a list of etudiants (students)
  public getEtudiants() : Observable<Etudiant[]>{
    return this.http.get<Etudiant[]>(`${environment.apiUrl}/${this.url}`);
  }
  public exportXlsx(selectedDataColIds: number[]):boolean{

    const queryParams = selectedDataColIds.map(id => `selectedDataColIds=${id}`).join('&');
    //consume the api via it link
    this.http.get(`${environment.apiUrl}/${this.url}/${this.exportURL_XLSX+"?"}`+ queryParams, { responseType: 'blob' })
    //response
      .subscribe(response => {
        const filename = 'etudiants.xlsx';
        const file = new Blob([response], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        //process of creating an href link then click it to autodownload the file
        const fileURL = URL.createObjectURL(file);
        const link = document.createElement('a');
        link.href = fileURL;
        link.download = filename;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        URL.revokeObjectURL(fileURL);
        return true;
      }, error => {
        console.error('Error occurred during file export:', error);
        return false
      });
  return true;
  }
  public exportPdf(selectedDataColIds: number[]):boolean{

    const queryParams = selectedDataColIds.map(id => `selectedDataColIds=${id}`).join('&');
    this.http.get(`${environment.apiUrl}/${this.url}/${this.exportURL_PDF+"?"}`+ queryParams, { responseType: 'blob' })
    .subscribe(response => {
      const filename = 'etudiants.pdf'; 
      const file = new Blob([response], { type: 'application/pdf' });
      const fileURL = URL.createObjectURL(file);
      const link = document.createElement('a');
      link.href = fileURL;
      link.download = filename;
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      URL.revokeObjectURL(fileURL);
      return true;
    }, error => {
      console.error('Error occurred during file export:', error);
      return false;
    }
    );
    return true;
  }
  public updateEtudiant(etd : Etudiant) : Observable<Etudiant[]>{
    return this.http.put<Etudiant[]>(`${environment.apiUrl}/${this.url}`, etd);
  }
  public async createEtudiant(etd : Etudiant){
    return await this.http.post<Etudiant[]>(`${environment.apiUrl}/${this.url}`, etd);
  }
  public async deleteEtudiant(etd : Etudiant) {
    return await this.http.delete(`${environment.apiUrl}/${this.url}/${etd.id}`);
  }
}
