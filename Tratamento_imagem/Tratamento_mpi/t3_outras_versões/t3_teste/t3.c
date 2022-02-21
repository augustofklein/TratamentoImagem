#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <string.h>
#include <mpi.h>

/*---------------------------------------------------------------------*/
#pragma pack(1)
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

FILE *fin = NULL;
FILE *fout = NULL;

int altura = 400;
int largura = 600;

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

RGB preenche_matriz(RGB m[altura][largura], int nproc, int id, char entrada[100]){

	int i,j;
	RGB pixel, aux;
	CABECALHO cabecalho;

	FILE *aux_file = fopen(entrada, "rb");
	
	if ( aux_file == NULL ){
		printf("Erro ao abrir o arquivo %s\n", entrada);
		exit(0);
	}

	fread(&cabecalho, sizeof(CABECALHO), 1, aux_file);

	for(i=0;i<altura;i++){
		int ali = (altura * 3) % 4;

		if (ali != 0){
			ali = 4 - ali;
		}
		
		for(j=id;j<largura;j+=nproc){
			fread(&pixel, sizeof(RGB), 1, aux_file);
			m[i][j] = pixel;
		}
		
		for(j=0; j<ali; j++){
			fread(&aux, sizeof(unsigned char), 1, aux_file);
		}
		
	}

	fclose(aux_file);

}

RGB calcula_mediana(RGB m[altura][largura], int nproc, int id, int mascara){

	int i, j, k, qtd_posi;

	RGB aux;

	qtd_posi = (mascara-1)*4 + 1;
	// Faz o cálculo da mediana levando em conta todos os tipos de máscaras possíveis
	for(i=(mascara-1); i<(altura-(mascara-1)); i++){
		for(j=(mascara+id-1) ; j<=(largura-(mascara-1)) ; j+=nproc){
			aux.blue = m[i][j].blue;
			aux.green = m[i][j].green;
			aux.red = m[i][j].red;
			for(k=1;k<mascara;k++){
				aux.green =+ m[i][j+k].green + m[i][j-k].green + m[i+k][j].green + m[i-k][j].green; 
				aux.blue  =+ m[i][j+k].blue  + m[i][j-k].blue  + m[i+k][j].blue  + m[i-k][j].blue;
				aux.red   =+ m[i][j+k].red   + m[i][j-k].red   + m[i+k][j].red   + m[i-k][j].red; 
			}
			aux.blue /= qtd_posi;
			aux.green /= qtd_posi;
			aux.red /= qtd_posi;
			m[i][j].blue = aux.blue;
			m[i][j].green = aux.green;
			m[i][j].red = aux.red;
		}
	}
}

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
	CABECALHO cabecalho;
	int i, j;
	int id, nproc;
	int tam_mascara;
	short media;
	char aux;
	MPI_Status s;
	RGB pixel;
	RGB m[altura][largura];

	tam_mascara = 1;

	//Inicialização do MPI
    MPI_Init(&argc, &argv); //Inicialização fazendo a passagem dos parâmetros
    MPI_Comm_rank(MPI_COMM_WORLD, &id); //Geração de um ID para cada processo
    MPI_Comm_size(MPI_COMM_WORLD, &nproc); //Agrupa os processos dentro de uma mesma área 

	zera_matriz(m);

	// Preenchimento dos pixeis pelo processo de MPI
	for(i=0;i<nproc;i++){
		if (id == i){
			// Primeiro elemento apenas envia e não recebe
			if (i == 0){
				preenche_matriz(m, nproc, id, "b2.bmp");
				// Caso houver mais de um processo
				if (nproc > 1){
					MPI_Send(&m, sizeof(m), MPI_UNSIGNED_CHAR, i+1, 100, MPI_COMM_WORLD);
				}
			}else{
				MPI_Recv(&m, sizeof(m), MPI_UNSIGNED_CHAR, i-1, 100, MPI_COMM_WORLD, &s);
				preenche_matriz(m, nproc, id, "b2.bmp");
				if (nproc-1 != i){
					MPI_Send(&m, sizeof(m), MPI_UNSIGNED_CHAR, i+1, 100, MPI_COMM_WORLD);
				}
			}
		}
	}

	// Relização do cálculo da mediana
	for(i=0;i<nproc;i++){
		if (id == i){
			// Primeiro elemento apenas envia e não recebe
			if (i == 0){
				calcula_mediana(m, nproc, id, tam_mascara);
				// Caso houver mais de um processo
				if (nproc > 1){
					MPI_Send(&m, sizeof(m), MPI_UNSIGNED_CHAR, i+1, 100, MPI_COMM_WORLD);
				}
			}else{
				MPI_Recv(&m, sizeof(m), MPI_UNSIGNED_CHAR, i-1, 100, MPI_COMM_WORLD, &s);
				calcula_mediana(m, nproc, id, tam_mascara);
				if (nproc-1 != i){
					MPI_Send(&m, sizeof(m), MPI_UNSIGNED_CHAR, i+1, 100, MPI_COMM_WORLD);
				}
			}
		}	
	}

	// Primeiro fazer teste apenas de um processo
	if(id == 0){

		fin = fopen(entrada, "rb");

		if ( fin == NULL ){
			printf("Erro ao abrir o arquivo %s\n", "b.bmp");
			exit(0);
		}

		fout = fopen(saida, "wb");

		if ( fout == NULL ){
			printf("Erro ao abrir o arquivo %s\n", "bb.bmp");
			exit(0);
		}

		// Leitura do cabeçalho original
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
	}

	MPI_Finalize();

	return 0;

}