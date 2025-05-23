<template>
  <div id="board"></div>
</template>

<script lang="ts">
import { Chess, type PieceSymbol, type Color, SQUARES, WHITE, BLACK, PAWN, KNIGHT, BISHOP, ROOK, QUEEN, KING } from 'chess.js'
import * as BUS from '@/utils/bus'
import { BoardChangeEvent } from '@/events/BoardChangeEvent'

export default {
  data() {
    return {
      chess: new Chess(),
      pov: WHITE,
      squareSize: 0,
      movingPieceId: '',
      cursorPositionX: 0,
      cursorPositionY: 0,
    }
  },
  mounted() {
    window.addEventListener('resize', this.onResize)
    window.addEventListener('mousemove', this.onMouseMove)
    this.drawBoard()
  },
  unmounted() {
    window.removeEventListener('resize', this.onResize)
    window.removeEventListener('mousemove', this.onMouseMove)
  },
  methods: {
    setPgn(pgn: string, pov: string) {
      this.pov = pov
      this.chess.loadPgn(pgn)
      this.drawBoard()
    },
    makeMove(move: string) {
      let moves = this.chess.moves({ verbose: true })
      for (let i = 0; i < moves.length; i++) {
        if (moves[i].lan == move) {
          this.chess.move(moves[i])
          this.drawBoard()

          break
        }
      }
    },
    drawBoard() {
      let board = document.getElementById('board')
      let widthMargin = window.innerWidth > 1024 ? 650 : 0
      let heightMargin = window.innerWidth > 1024 ? 150 : 260

      let windowWidth = window.innerWidth - widthMargin
      let windowHeight = window.innerHeight - heightMargin

      this.squareSize = Math.floor(Math.min(windowWidth / 8, windowHeight / 8))

      board!.innerHTML = ''
      board!.style.width = this.squareSize * 8 + 'px'
      board!.style.height = this.squareSize * 8 + 'px'

      for (let file = 0; file < 8; file++) {
        for (let rank = 0; rank < 8; rank++) {
          let square = this.getSquareByFileRank(file, rank)
          let squareElement = document.createElement('div')
          let squareImageUrl = this.getSquareImageUrl(file, rank)
          let squareName = this.getSquareName(square)

          squareElement.id = 'S' + squareName
          squareElement.className = 'square'
          squareElement.style.width = this.squareSize + 'px'
          squareElement.style.height = this.squareSize + 'px'
          squareElement.style.transform = 'translate(' + file * this.squareSize + 'px, ' + rank * this.squareSize + 'px)'
          squareElement.style.backgroundImage = 'url(' + squareImageUrl + ')'
          squareElement.setAttribute('data-square', squareName)
          board?.appendChild(squareElement)

          let piece = this.chess.get(SQUARES[square])
          if (piece != undefined) {
            let pieceElement = document.createElement('div')
            let pieceImageUrl = this.getPieceImageUrl(piece.color, piece.type)

            pieceElement.id = 'P' + squareName
            pieceElement.className = 'piece'
            pieceElement.style.width = this.squareSize + 'px'
            pieceElement.style.height = this.squareSize + 'px'
            pieceElement.style.transform = 'translate(' + file * this.squareSize + 'px, ' + rank * this.squareSize + 'px)'
            pieceElement.style.backgroundImage = 'url(' + pieceImageUrl + ')'
            pieceElement.setAttribute('data-square', squareName)
            pieceElement.addEventListener('mousedown', this.onPieceMouseDown)
            pieceElement.addEventListener('mouseup', this.onPieceMouseUp)
            board?.appendChild(pieceElement)
          }
        }
      }
    },
    getSquareByFileRank(file: number, rank: number) {
      switch (this.pov) {
        case WHITE:
          return file + rank * 8
        case BLACK:
          return file + (7 - rank) * 8
        default:
          return -1
      }
    },
    getSquareByCursorPosition(x: number, y: number) {
      let board = document.getElementById('board')!
      let offsetX = x - board.offsetLeft
      let offsetY = y - board.offsetTop
      let file = Math.floor(offsetX / this.squareSize)
      let rank = Math.floor(offsetY / this.squareSize)

      if (file < 0 || file >= 8 || rank < 0 || rank >= 8) {
        return -1
      }

      return this.getSquareByFileRank(file, rank)
    },
    getSquareName(square: number) {
      let file = square % 8
      let rank = Math.floor(square / 8)

      return String.fromCharCode('a'.charCodeAt(0) + file) + (8 - rank)
    },
    isSquareValid(square: number) {
      return square >= 0 && square < 64
    },
    getSquareImageUrl(file: number, rank: number) {
      let odd = (file + rank + 1) % 2 != 0

      if (this.pov == BLACK) {
        odd = !odd
      }

      return odd ? '/assets/squares/white.svg' : '/assets/squares/black.svg'
    },
    getPieceImageUrl(color: Color, type: PieceSymbol) {
      if (color == WHITE) {
        switch (type) {
          case PAWN:
            return '/assets/pieces/pawn_white.svg'
          case KNIGHT:
            return '/assets/pieces/knight_white.svg'
          case BISHOP:
            return '/assets/pieces/bishop_white.svg'
          case ROOK:
            return '/assets/pieces/rook_white.svg'
          case QUEEN:
            return '/assets/pieces/queen_white.svg'
          case KING:
            return '/assets/pieces/king_white.svg'
        }
      } else {
        switch (type) {
          case PAWN:
            return '/assets/pieces/pawn_black.svg'
          case KNIGHT:
            return '/assets/pieces/knight_black.svg'
          case BISHOP:
            return '/assets/pieces/bishop_black.svg'
          case ROOK:
            return '/assets/pieces/rook_black.svg'
          case QUEEN:
            return '/assets/pieces/queen_black.svg'
          case KING:
            return '/assets/pieces/king_black.svg'
        }
      }
    },
    onResize() {
      this.drawBoard()
    },
    onMouseMove(e: MouseEvent) {
      if (this.movingPieceId != '') {
        let pieceElement = document.getElementById(this.movingPieceId)!

        pieceElement.style.transform = ''
        pieceElement.style.left = e.clientX - this.squareSize / 2 + 'px'
        pieceElement.style.top = e.clientY - this.squareSize / 2 + 'px'
        pieceElement.style.zIndex = '20'
      }

      this.cursorPositionX = e.clientX
      this.cursorPositionY = e.clientY
    },
    onPieceMouseDown(e: MouseEvent) {
      this.movingPieceId = (e.target as Element).id

      let piece = document.getElementById(this.movingPieceId)
      let from = piece?.getAttribute('data-square')
      let moves = this.chess.moves({ verbose: true }).filter((p) => p.from == from)

      if (moves.length > 0) {
        if (this.chess.get(moves[0].from)?.color == this.pov) {
          for (let i = 0; i < moves.length; i++) {
            let squareElement = document.getElementById('S' + moves[i].to)!
            squareElement.className = 'square square-candidate'
          }
        }
      }
    },
    onPieceMouseUp(e: MouseEvent) {
      let piece = document.getElementById(this.movingPieceId)
      let square = this.getSquareByCursorPosition(this.cursorPositionX, this.cursorPositionY)
      this.movingPieceId = ''

      if (!this.isSquareValid(square)) {
        this.drawBoard()
        return
      }

      let moves = this.chess.moves({ verbose: true })
      let from = piece?.getAttribute('data-square')
      let to = this.getSquareName(square)

      for (let i = 0; i < moves.length; i++) {
        if (moves[i].from == from && moves[i].to == to) {
          this.chess.move(moves[i])

          BUS.emitter.emit('onBoardChange', {
            move: moves[i].lan,
          })
        }
      }

      this.movingPieceId = ''
      this.drawBoard()
    },
  },
}
</script>

<style>
.square {
  position: absolute;
  background-size: 100% 100%;
}

.square-candidate {
  filter: brightness(0.5);
}

.piece {
  position: absolute;
  background-size: 100% 100%;
  z-index: 10;
}
</style>
