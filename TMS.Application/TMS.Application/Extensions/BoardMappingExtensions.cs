﻿using TMS.Application.Dto.Board;
using TMS.Infrastructure.DataModels;

namespace TMS.Application.Extensions
{
    public static class BoardMappingExtensions
    {
        public static Board ToBoard(this BoardCreateDto dto)
        {
            return new Board
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                CompanyId = dto.CompanyId,
                IsPrivate = dto.IsPrivate,
                CreationDate = DateTime.UtcNow,
            };
        }

        public static BoardDto ToBoardDto(this Board board)
        {
            return new BoardDto
            {
                Id = board.Id,
                Name = board.Name,
                Description = board.Description,
                CompanyId = board.CompanyId,
                IsPrivate = board.IsPrivate,
                CreationDate = board.CreationDate,
                UpdateDate = board.UpdateDate,
                DeleteDate = board.DeleteDate
            };
        }
    }
}