#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <string.h>
#include <mpi.h>
/*---------------------------------------------------------------------*/
#pragma pack(1)

//Compilar:
//mpicc nome.c -o nome

//Executar:
//mpirun -np <nproc> ./nome

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

	for(i=0;i<(altura/nproc)*(id);i++){
		for(j=0;j<largura;j++){
			fread(&pixel, sizeof(RGB), 1, aux_file);
		}
	}

	for(i=(altura/nproc)*id ; i<(altura/nproc)*(id+1) ; i++){
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

// FUnção para realizar o cálculo da mediana sobre a matriz
RGB calcula_mediana(RGB m[altura][largura], int nproc, int id, int mascara){

	RGB media, aux;
	RGB *vet;
	int qtd, ini, fim, k, l, linha;
	int i, j, qtd_posi, cont;

	if(mascara == 3){
		linha = 1;
	}else if(mascara == 5){
		linha = 2;
	}else{
		linha = 3;
	}

	qtd = mascara * mascara;
	ini = (altura/nproc)*id;
	fim = (altura/nproc)*(id+1)-(mascara-(mascara-1));

	for(i=ini; i<=fim; i++){
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
	CABECALHO cabecalho;
	int i, j;
	int id, nproc;
	int tam_mascara;
	short media;
	char aux;
	MPI_Status s;
	RGB pixel;
	RGB m[altura][largura];

	tam_mascara = 7;

	//Inicialização do MPI
    MPI_Init(&argc, &argv); //Inicialização fazendo a passagem dos parâmetros
    MPI_Comm_rank(MPI_COMM_WORLD, &id); //Geração de um ID para cada processo
    MPI_Comm_size(MPI_COMM_WORLD, &nproc); //Agrupa os processos dentro de uma mesma área 

	// Inicializa a matriz com '0' em todas as posições
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

	// Atualização da matriz em todos os processos
	for(i=(nproc-1); i>=0 ;i--){
		if(id == i){
			// Se for o último processo
			if((i < nproc-1) && (nproc > 1)){
				MPI_Recv(&m, sizeof(m), MPI_UNSIGNED_CHAR, (i+1), 100, MPI_COMM_WORLD, &s);
			}
			if(i != 0){
				MPI_Send(&m, sizeof(m), MPI_UNSIGNED_CHAR, (i-1), 100, MPI_COMM_WORLD);
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

	// Impressão do arquivo de saída
	if(id == (nproc-1)){

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