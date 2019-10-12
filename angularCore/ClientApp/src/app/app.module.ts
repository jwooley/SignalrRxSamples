import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { ReactiveChatComponent } from './reactiveChat/reactiveChat.component';
import { SensorComponent } from './sensor/sensor.component';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { DragDropComponent } from './dragDrop/dragDrop.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    ReactiveChatComponent,
        SensorComponent,
    DragDropComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
      ReactiveFormsModule,
    DragDropModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'reactiveChat', component: ReactiveChatComponent },
      { path: 'fetch-data', component: FetchDataComponent },
        { path: 'sensor', component: SensorComponent },
      { path: 'dragdrop', component: DragDropComponent }
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
