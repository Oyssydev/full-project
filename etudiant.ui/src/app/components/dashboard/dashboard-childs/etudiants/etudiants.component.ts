import { Component, Injectable ,TemplateRef, OnInit, ViewChild, Output, EventEmitter, ElementRef } from "@angular/core";
import { Etudiant } from "src/app/models/etudiants";
import { EtudiantService } from "src/app/services/etudiant.service";
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { AuthService } from "src/app/services/auth.service";

@Injectable({
  providedIn: 'root'
})
@Component({
  selector: 'app-etd',
  templateUrl: './etudiants.component.html',
  styleUrls: ['./etudiants.component.css']
})
export class EtudiantsComponent {

  @Output() etdUpdate = new EventEmitter<Etudiant[]>();
  title = 'etudiant.ui';
  etds : Etudiant[] = [];
  public etdToEdit?: Etudiant;
  modalRef: BsModalRef = {} as BsModalRef;
  public etudiant?: any  = {};
  alertVisible = false;
  public alertType!: string;
  public alertMessage!: string;
  public selectedDataCols : number[] = [1];

  
  constructor(private etudiantService : EtudiantService,private modalService: BsModalService,public authService : AuthService){ 
  }

  ngOnInit() : void{
    this.getAll();
  }
  openDialoge(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }
  editEtudiant(etd: Etudiant){
    this.etudiant = etd;

  }
  handleCheckboxChange(event: Event) {
    const checkbox = event.target as HTMLInputElement;
    if (checkbox.checked) {
      this.selectedDataCols.push(parseInt(checkbox.value, 10));
    } 
    else {

      this.selectedDataCols.forEach((element,index)=>{
        if(element==parseInt(checkbox.value, 10)) this.selectedDataCols.splice(index,1);
     });
    }
  }
  async deleteEtd(etd:Etudiant){  
    let result = await (await this.etudiantService.deleteEtudiant(etd))
    result.subscribe(async (res) =>{
      let result = res as boolean;    
      if(result){
        this.getAll();
     }
    })
  }
  async createEtd(etd:Etudiant){
    let result = await (await this.etudiantService.createEtudiant(etd))
    result.subscribe(async (res) =>{
      let result = res as Etudiant[];   
      if(result != null){        
        this.getAll();
      }
   })
  }
  updateEtd(etd:Etudiant){
    this.etudiantService
    .updateEtudiant(etd)
    .subscribe((etudiants:Etudiant[])=>this.etdUpdate.emit(etudiants));
  }
  getAll(){
    this.etudiantService.getEtudiants().subscribe((result : Etudiant[])=> (this.etds = result));
  }
 
  razEtudiant(){
    this.etudiant = {};
  }

  showAlert(alertType:string, alertMessage:string){
    this.alertMessage = alertMessage;
    this.alertType = alertType;
    this.alertVisible = true;
  }
  closeAlert(){
    this.alertVisible = false;
  }
  openPopUp(){
    this.selectedDataCols = [1];
  }

  exportXlsx(selectedDataCols:number[]) {
    if(this.etudiantService.exportXlsx(selectedDataCols)){
      //has a result
      this.showAlert("success", "Excel File downloaded succesfully");
    }else{
      this.showAlert("danger", "Error");
    }
  }

  exportPdf(selectedDataCols:number[]){
    if(this.etudiantService.exportPdf(selectedDataCols)){
      //has a result
      this.showAlert("success", "PDF File downloaded succesfully");
    }else{
      this.showAlert("danger", "Error");
    }
  }
}
