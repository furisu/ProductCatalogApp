﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ProductCatalogApp.Models
{
    public class Product
    {
        public int Id { get; set; } // 主キー（自動採番）

        [Required(ErrorMessage = "製品名は必須です")]
        [StringLength(100, ErrorMessage = "製品名は100文字以内で入力してください")]
        public string Name { get; set; }

        [Required(ErrorMessage = "カテゴリは必須です")]
        public int CategoryId { get; set; }  // 外部キー
        [ValidateNever]
        public Category Category { get; set; }  // ナビゲーションプロパティ

        [Required(ErrorMessage = "価格は必須です")]
        [Range(0, 100000, ErrorMessage = "価格は0円〜100,000円の範囲で入力してください")]
        public int Price { get; set; }

        [StringLength(500, ErrorMessage = "説明は500文字以内で入力してください")]
        public string? Description { get; set; }

        [Url(ErrorMessage = "正しいURL形式で入力してください")]
        public string? ImageUrl { get; set; }

        [Range(0, 9999, ErrorMessage = "在庫数は0〜9999の範囲で入力してください")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "販売状況は必須です")]
        public int StatusId { get; set; }  // 外部キー
        [ValidateNever]
        public Status Status { get; set; }  // ナビゲーションプロパティ

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; }
    }
}


