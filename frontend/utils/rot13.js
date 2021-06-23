export const ROT13 = (value) => {
    var s = []
    for(var i = 0; i < value.length; i++){
        var j = value.charCodeAt(i)

        if((j >= 65) && (j <= 90)){
            s[i] = String.fromCharCode(65+((j - 65 + 13)%26))
        } else {
            //s[i] = String.fromCharCode(j)
        }
    }
    return s.join('')
}