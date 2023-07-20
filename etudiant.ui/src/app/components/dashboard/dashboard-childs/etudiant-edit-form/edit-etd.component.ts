import { Component, EventEmitter, Inject, Injectable, Input, OnInit, Output, inject } from "@angular/core";
import { Etudiant } from "src/app/models/etudiants";
import { EtudiantService } from "src/app/services/etudiant.service";
import { EtudiantsComponent } from "../etudiants/etudiants.component";
@Injectable({
  providedIn:'root'
})
@Component({
  selector: 'app-edit-etd',
  templateUrl: './edit-etd.component.html',
  styleUrls: ['./edit-etd.component.css']
})
export class EditEtdComponent implements OnInit{

  etudiant?: Etudiant;
  @Output() etdUpdate = new EventEmitter<Etudiant[]>();

  constructor(private etdService:EtudiantService, private etdCmp: EtudiantsComponent){
  };

  ngOnInit():void{
  }

  updateEtd(etd:Etudiant){
    this.etdService
    .updateEtudiant(etd)
    .subscribe((etudiants:Etudiant[])=>this.etdUpdate.emit(etudiants));
  }
  createEtd(etd:Etudiant){
    this.etdCmp.createEtd(etd);
  }
}
