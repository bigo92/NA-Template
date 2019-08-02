using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace tci.common.Enums
{
    public class Enums
    {
        public enum Status_db
        {
            [Display(Name = "status_db.notactive")]
            NotActive,
            [Display(Name = "status_db.nomal")]
            Nomal,
            [Display(Name = "status_db.hiden")]
            Hide,
            [Display(Name = "status_db.delete")]
            Delete,
            [Display(Name = "status_db.deleted")]
            Deleted,
        }

        public enum UserStatus
        {
            [Display(Name = "NotActive")]
            NotActive,
            [Display(Name = "LogOff")]
            LogOff,
            [Display(Name = "Online")]
            Online,
            [Display(Name = "Blocked")]
            Blocked,
        }

        public enum UserType
        {
            [Display(Name = "Admin")]
            Admin,
            [Display(Name = "User")]
            User
        }

        public enum GoodsVoucherStatus
        {
            [Display(Name = "New")]
            New = 1,
            [Display(Name = "Done")]
            Done,
            [Display(Name = "Change")]
            Edit,
            [Display(Name = "Packaging")]
            Packaging,
            [Display(Name = "Delivering")]
            Delivering,
            [Display(Name = "Cancel")]
            Cancel,
            [Display(Name = "OnHold")]
            OnHold,
            [Display(Name = "Transfering")]
            Transfering,
            [Display(Name = "TransferCompleted")]
            TransferCompleted,
            [Display(Name = "Invoke")]
            Invoke,
        }


        public enum GoodsVoucherType
        {
            [Display(Name = "Receipt")]
            Receipt = 1,
            [Display(Name = "Transfer")]
            Transfer,
            [Display(Name = "Issue")]
            Issue,
            [Display(Name = "Expenses Payable")]
            PaymentExpenses,
            [Display(Name = "Receive Payments")]
            PaymentReceive
        }

        public enum GoodsVoucherType2
        {
            [Display(Name = "Order Original")]
            Original = 1,
            [Display(Name = "Order Correction")]
            Correction
        }

        public enum GoodsVoucherIssueType
        {
            [Display(Name = "Order new")]
            New = 1,
            [Display(Name = "Order Return")]
            Return,
        }

        public enum GoodsVoucherMode
        {
            [Display(Name = "Nomal")]
            Nomal,
            [Display(Name = "Assign")]
            Assign,
            [Display(Name = "Update")]
            Update,
            [Display(Name = "Confirm")]
            Confirm
        }

        public enum TypeData
        {
            [Display(Name = "Number")]
            Number = 1,
            [Display(Name = "String")]
            String,
            [Display(Name = "Date")]
            Date,
            [Display(Name = "Float")]
            Float,
            [Display(Name = "Dropdown Single")]
            DropdownSingle,
            [Display(Name = "Boolean")]
            Boolean,
            [Display(Name = "Textarea")]
            Textarea,
            [Display(Name = "Editor")]
            Editor,
            [Display(Name = "Image")]
            Image,
            [Display(Name = "Tag")]
            Tag,
            [Display(Name = "DropdownMultipe")]
            DropdownMultipe,
            [Display(Name = "Email")]
            Email,
            [Display(Name = "Phone")]
            Phone,
            [Display(Name = "File")]
            File,
            [Display(Name = "TableSingle")]
            TableSingle,
            [Display(Name = "JObject")]
            JObject,
            [Display(Name = "Jaray")]
            Jaray,
            [Display(Name = "TableMultipe")]
            TableMultipe,
            [Display(Name = "DateTime")]
            DateTime,
            [Display(Name = "JSON")]
            JSON,
        }

        public enum UnitType
        {
            Unit,
            Additional
        }

        public enum PaymentType
        {
            CreateByNomal,
            CreateByVoucherReceipt,
            CreateByVoicherTransfer,
            CreateByVoicherIssue
        }

        public enum PaymentMethod
        {
            Cash = 1,
            Bank
        }

        public enum PaymentStatus
        {
            NeedConfirm,
            Confirmed
        }

        public enum KomunikatKod
        {
            NeedCaptcha = 1,
            TooManyIds = 2,
            NoSubjects = 4,
            NoSession = 7
        }

        public enum Method
        {
            PobierzCaptcha,
            SprawdzCaptcha,
            Zaloguj,
            Wyloguj,
            DaneSzukaj,
            DanePobierzPelnyRaport,
            DaneKomunikat,
            GetValue
        }

        public enum RaportFiz
        {
            //DaneRaportFizycznaPubl,
            //DaneRaportDzialalnosciFizycznejPubl,
            //DaneRaportLokalneFizycznejPubl
            PublDaneRaportFizycznaOsoba,
            PublDaneRaportDzialalnoscFizycznejCeidg,
            PublDaneRaportDzialalnoscFizycznejRolnicza,
            PublDaneRaportDzialalnoscFizycznejPozostala,
            PublDaneRaportDzialalnoscFizycznejWKrupgn,
            PublDaneRaportLokalneFizycznej,
            PublDaneRaportLokalnaFizycznej,
            PublDaneRaportDzialalnosciFizycznej,
            PublDaneRaportDzialalnosciLokalnejFizycznej
        }

        public enum RaportPraw
        {
            //DaneRaportPrawnaPubl,   
            //DaneRaportDzialalnosciPrawnejPubl,
            //DaneRaportLokalnePrawnejPubl,
            //DaneRaportWspolnicyPrawnejPubl
            PublDaneRaportPrawna,
            PublDaneRaportDzialalnosciPrawnej,
            PublDaneRaportLokalnePrawnej,
            PublDaneRaportLokalnaPrawnej,
            PublDaneRaportDzialalnosciLokalnejPrawnej,
            PublDaneRaportWspolnicyPrawnej,
            PublDaneRaportTypJednostki
        }

        public enum SearchBy
        {
            Nip,
            Regon,
            Krs,
            Nipy,
            Regony9zn,
            Rregony14zn,
            Krsy
        }

        public enum ServiceStatus
        {
            StanDanych,
            KomunikatKod,
            KomunikatTresc,
            StatusSesji,
            StatusUslugi,
            KomunikatUslugi
        }
    }
}
