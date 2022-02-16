# Componentes

## Criar componente
ng g c nome-componente

## Criar componente resumido
ng g c nome-componente --flat --nospec

## Criar componente em subdiretorio
ng g c subdiretorio\nome-componente --flat --nospec

## Gerar imagens de 1x1 pixel
http://www.1x1px.me

## Baixar fonts
https://fonts.google.com/specimen/Titillium+Web


## Para adicionar arquivos .css sem encapsular no componente
Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.css',
      '../../assets/styles/normalize.css',
      '../../assets/styles/animate.css',
      '../../assets/styles/font-awesome.css',
      '../../assets/styles/bootstrap-datetimepicker.css',
      '../../assets/styles/main.css'],
  encapsulation: ViewEncapsulation.None // https://stackoverflow.com/questions/40116678/angular2-styleurls-not-loading-external-styles
})
