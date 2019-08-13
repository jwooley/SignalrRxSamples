import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';

import { SecurityService } from './services/security';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { ReactiveChatComponent } from './reactiveChat/reactiveChat.component';
import { ReactiveFormComponent } from './reactiveForm/reactiveForm.component';
import { SensorComponent } from './sensor/sensor.component';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    ReactiveChatComponent,
    ReactiveFormComponent,
    SensorComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'reactiveChat', component: ReactiveChatComponent },
      { path: 'reactiveForm', component: ReactiveFormComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'sensor', component: SensorComponent}
    ])
  ],
  providers: [SecurityService],
  bootstrap: [AppComponent]
})
export class AppModule { }
