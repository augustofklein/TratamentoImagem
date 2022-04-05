#include <stdio.h>
#include <stdlib.h>
#include <omp.h>
#define NTHREADS 5
/*---------------------------------------------------------------------*/
#pragma pack(1)
/*---------------------------------------------------------------------*/
// COMPILAÇÃO: gcc -o prog prog.c -fopenmp
//   EXECUÇÃO: ./prog
/*---------------------------------------------------------------------*/
struct cabecalho {
	unsigned short tipo;
	unsigned int tamanho_arquivo;
	unsigned short reservado1;
	unsigned short reservado2;
	unsigned int offset;
	unsigned int tamanho_image_header;
	int largura;
	int altura;
	unsigned short planos;
	unsigned short bits_por_pixel;
	unsigned int compressao;
	unsigned int tamanho_imagem;
	int largura_resolucao;
	int altura_resolucao;
	unsigned int numero_cores;
	unsigned int cores_importantes;
}; 
typedef struct cabecalho CABECALHO;

struct rgb{
	unsigned char blue;
	unsigned char green;
	unsigned char red;
};
typedef struct rgb RGB;

int altura = 400;
int largura = 600;

// Função para zerar toda matriz
RGB zera_matriz(RGB m[altura][largura]){

	int i,j;

	for(i=0;i<altura;i++){
		for(j=0;j<largura;j++){
			m[i][j].blue  = 0;
			m[i][j].green = 0;
			m[i][j].red   = 0;
		}
	}
}

// Função de preenchimento da matriz com os pixeis da imagem
RGB preenche_matriz(RGB m[altura][largura], char entrada[100]){

	int i,j;
	RGB pixel, aux;
	CABECALHO cabecalho;

	FILE *aux_file = fopen(entrada, "rb");
	
	if ( aux_file == NULL ){
		printf("Erro ao abrir o arquivo %s\n", entrada);
		exit(0);
	}

	fread(&cabecalho, sizeof(CABECALHO), 1, aux_file);

	for(i=0 ; i<altura ; i++){
		int ali = (altura * 3) % 4;

		if (ali != 0){
			ali = 4 - ali;
		}
		
		for(j=0;j<largura;j++){
			fread(&pixel, sizeof(RGB), 1, aux_file);
			m[i][j] = pixel;
		}
		
		for(j=0; j<ali; j++){
			fread(&aux, sizeof(unsigned char), 1, aux_file);
		}
		
	}

	fclose(aux_file);

}

// Função para realizar o cálculo da mediana sobre a matriz
RGB calcula_mediana(RGB m[altura][largura], int mascara){

	RGB aux, *vet;
	int i, j, k, l, cont, qtd, ini, fim, linha, threads, parte, id;

	ini = 0;
	fim = 0;
	parte = 0;

	qtd = mascara * mascara;

	if(mascara == 3){
		linha = 1;
	}else if(mascara == 5){
		linha = 2;
	}else{
		linha = 3;
	}

	// Setado o número de threads
	omp_set_num_threads(NTHREADS);
	// Cria multithreads
	// ID e THREADS, cada thread realiza uma cópia delas variáveis
	#pragma omp parallel private (id, parte, ini, fim, i, j, k, l, vet, cont, aux)
	{
		// É dividido a quantidade de linhas da matriz pela quantidade de threadss
		parte = altura / omp_get_num_threads();

		// Pegar o número do id de cada thread
		id = omp_get_thread_num();

		// Fazer a divisão da matriz pela quantidade de processos
		ini = parte * id;
		fim = ini + parte;

		for(i=ini; i<fim; i++){
			for(j=mascara/2 ; j<=(largura-(mascara-1)) ; j++){
				vet = (RGB*)malloc(qtd * sizeof(RGB));
				cont = 0;
				for(k=i-linha; k<(i-linha+mascara); k++){
					for(l=j-linha; l<(j-linha+mascara); l++){
						vet[cont] = m[k][l];
						cont++;
					}
				}
				// Ordenação
				for(k=0;k<qtd;k++){
					for(l=0;l<qtd-1;l++){
						//Azul
						if(vet[l].blue > vet[l+1].blue){
							aux.blue = vet[l].blue;
							vet[l].blue = vet[l+1].blue;
							vet[l+1].blue = aux.blue;
						}
						//Verde
						if(vet[l].green > vet[l+1].green){
							aux.green = vet[l].green;
							vet[l].green = vet[l+1].green;
							vet[l+1].green = aux.green;
						}
						//Vermelho
						if(vet[l].red > vet[l+1].red){
							aux.red = vet[l].red;
							vet[l].red = vet[l+1].red;
							vet[l+1].red = aux.red;
						} 
					}
				}
				m[i][j].blue = vet[qtd/2].blue;
				m[i][j].green = vet[qtd/2].green;
				m[i][j].red = vet[qtd/2].red;
				free(vet);
			}
		}
	}
}

// Faz a impressão da matriz, levando em considerção uma cor
void imprime_matriz(RGB m[altura][largura]){

	int i,j;
	short media;

	for(i=0;i<altura;i++){
		for(j=0;j<largura;j++){
			printf("%d ", m[i][j].blue);
		}
		printf("\n");
	}

}

int main(int argc, char **argv){

	char entrada[100] = "b.bmp";
	char saida[100] = "bb.bmp";
	char aux;
	int i, j, tam_mascara;
	short media;
	RGB pixel, m[altura][largura];
	CABECALHO cabecalho;
	FILE *fin = NULL;
	FILE *fout = NULL;

	tam_mascara = 3;				 			           // Tamanho da máscara

	zera_matriz(m);								           // Inicializa a matriz com '0' em todas as posições

	preenche_matriz(m, "b2.bmp");				           // Preenche a matriz com os pixeis da imagem
	
	calcula_mediana(m, tam_mascara);                       // Realiza o cálculo de mediana na matriz usando OpenMP
	
	fin = fopen(entrada, "rb");					           // Geração do arquivo de saída

	if ( fin == NULL ){
		printf("Erro ao abrir o arquivo %s\n", "b.bmp");
		exit(0);
	}

	fout = fopen(saida, "wb");

	if ( fout == NULL ){
		printf("Erro ao abrir o arquivo %s\n", "bb.bmp");
		exit(0);
	}

	fread(&cabecalho, sizeof(CABECALHO), 1, fin);
	fwrite(&cabecalho, sizeof(CABECALHO), 1, fout);

	for(i=0;i<altura;i++){
		int ali = (largura * 3) % 4;
		short media;

		if (ali != 0){
			ali = 4 - ali;
		}

		for(j=0;j<largura;j++){
			media = (m[i][j].blue + m[i][j].green + m[i][j].red)/3;
			m[i][j].blue = media;
			m[i][j].green = media;
			m[i][j].red = media;
			fwrite(&m[i][j], sizeof(RGB), 1, fout);
		}
		
		for(j=0;j<ali;j++){
			fread(&aux, sizeof(unsigned char), 1, fin);
			fwrite(&aux, sizeof(unsigned char), 1, fout);
		}
	}
	
	fclose(fin);
	fclose(fout);

	return 0;

}