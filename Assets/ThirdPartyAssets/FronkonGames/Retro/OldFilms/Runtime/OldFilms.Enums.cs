////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Martin Bustos @FronkonGames <fronkongames@gmail.com>. All rights reserved.
//
// THIS FILE CAN NOT BE HOSTED IN PUBLIC REPOSITORIES.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace FronkonGames.Retro.OldFilms
{
  /// <summary> Film manufacturers with authentic Color Decision List (CDL) characteristics. </summary>
  public enum Manufacturers
  {
    /// Original Kodak cinema film stocks

    /// <summary> Kodak 2383 - Classic cinema print film stock. </summary>
    Kodak_2383,
    /// <summary> Kodak 2393 - Color negative cinema film with warm tones. </summary>
    Kodak_2393,
    /// <summary> Kodak 2395 - Intermediate cinema film stock. </summary>
    Kodak_2395,

    /// Modern Kodak film stocks

    /// <summary> Kodak Ektar 100 - Ultra-vivid color negative film, excellent for nature and travel photography. Fine grain, high saturation. </summary>
    Kodak_Ektar_100,
    /// <summary> Kodak Portra 160 - Professional portrait film with natural skin tones and fine grain. Ideal for studio work. </summary>
    Kodak_Portra_160,
    /// <summary> Kodak Portra 400 - Versatile portrait film with excellent skin tone reproduction. Most popular professional portrait film. </summary>
    Kodak_Portra_400,
    /// <summary> Kodak Portra 800 - High-speed portrait film for low light conditions. Maintains good skin tones at higher ISO. </summary>
    Kodak_Portra_800,
    /// <summary> Kodak Ektachrome E100 - Color reversal slide film with neutral tones and excellent sharpness. </summary>
    Kodak_Ektachrome_E100,

    /// Agfa historical film stocks

    /// <summary> Agfa 1978 - Vintage color negative film with distinctive character. </summary>
    Agfa_1978,
    /// <summary> Agfa 1905 - Early black and white film stock with high contrast. </summary>
    Agfa_1905,
    /// <summary> Agfa 1935 - Early color film with warm, sepia-like tones. </summary>
    Agfa_1935,
    
    /// Modern Fujifilm stocks
    
    /// <summary> Fuji Pro 400H - Professional color negative with soft color reproduction and wide exposure latitude. Favored for portraits. </summary>
    Fuji_Pro_400H,
    /// <summary> Fuji Velvia 50 - Legendary landscape film with extremely high saturation and incredible sharpness. Cool color bias. </summary>
    Fuji_Velvia_50,
    /// <summary> Fuji Velvia 100 - Faster version of Velvia with rich, saturated colors. Slightly less contrast than Velvia 50. </summary>
    Fuji_Velvia_100,
    /// <summary> Fuji Provia 100F - Neutral color slide film for accurate color reproduction. Professional standard for commercial work. </summary>
    Fuji_Provia_100F,
    
    /// Cinestill motion picture films
    
    /// <summary> Cinestill 50D - Repurposed Kodak Vision3 cinema film for daylight shooting. Fine grain with cinematic look. </summary>
    Cinestill_50D,
    /// <summary> Cinestill 800T - Tungsten-balanced cinema film with characteristic halation and warm tones. Popular for night photography. </summary>
    Cinestill_800T,
    
    /// Lomography experimental films
    
    /// <summary> Lomography Color Negative 100 - Vibrant colors with characteristic Lomo look. Punchy saturation and unique color rendition. </summary>
    Lomography_Color_Negative_100,
    /// <summary> Lomography Color Negative 400 - Higher speed Lomo film with enhanced saturation and experimental color characteristics. </summary>
    Lomography_Color_Negative_400,

    /// Ilford black and white films
    
    /// <summary> Ilford HP5 Plus - Classic high-speed black and white film (ISO 400). Excellent for street and documentary photography. </summary>
    Ilford_HP5_Plus,
    /// <summary> Ilford FP4 Plus - Fine grain medium-speed black and white film (ISO 125). Ideal for portraits and detailed work. </summary>
    Ilford_FP4_Plus,
    /// <summary> Ilford Delta 100 - Modern tabular grain black and white film. Extremely fine grain and high sharpness. </summary>
    Ilford_Delta_100,
    /// <summary> Ilford Delta 400 - High-speed tabular grain black and white film. Excellent shadow detail and wide exposure latitude. </summary>
    Ilford_Delta_400,

    /// Beer film stocks (fictional/artistic interpretations)

    /// <summary> Beer 1973 - Artistic film interpretation with cool blue bias. </summary>
    Beer_1973,
    /// <summary> Beer 1933 - High contrast black and white artistic film. </summary>
    Beer_1933,
    /// <summary> Beer 2001 - Modern artistic film with muted colors. </summary>
    Beer_2001,
    /// <summary> Beer 2006 - Contemporary artistic film with enhanced contrast. </summary>
    Beer_2006,

    /// <summary> Polaroid - Instant film with characteristic warm, soft look. </summary>
    Polaroid,
      
    /// <summary> Cuba Libre - Artistic interpretation with warm, vintage tones. </summary>
    Cuba_libre,
      
    /// <summary> Fuji 4711 - Vintage Fuji film stock with muted saturation. </summary>
    Fuji_4711,

    /// <summary> ORWO 0815 - East German film stock with distinctive color characteristics. </summary>
    ORWO_0815,
      
    /// <summary> Black & White - Classic monochrome film look. </summary>
    Black_white,
      
    /// <summary> Spearmint - Artistic green-tinted film interpretation. </summary>
    Spearmint,
      
    /// <summary> Tea Time - Warm, sepia-toned artistic film look. </summary>
    Tea_time,
      
    /// <summary> Purple Rain - Artistic purple-tinted film interpretation. </summary>
    Purple_rain,
    
    /// <summary> Ferrania P30 - Classic Italian black and white film stock. High contrast with distinctive grain structure. </summary>
    Ferrania_P30,

    /// <summary> Custom - Allows manual adjustment of all CDL parameters for creating unique film looks. </summary>
    Custom,
  }
}
