export class CartModel {
    constructor(
        public id: string,
        public title: string,
        public quantity: number,
        public image: string,
        public price: any,
        public currency: string,
        public parent: string = null,
        public active: boolean = true,
        public max: number = 30
    ){}
}
