using OoxmlDemo.DocumentProcessing.Dto;
using OoxmlDemo.ExternalServices;
using OoxmlDemo.ExternalServices.Dto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Packaging;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OoxmlDemo.DocumentProcessing
{
    public abstract class DocumentContainer : PackageContainer
    {
        public DocumentContainer(string path) : base(path) { }


        protected TokenDescription FindToken( string text, int beginPosition = 0, string tokenType = null)
        {
            int idxStart;
            while ( (idxStart = text.IndexOf('$', beginPosition)) >= 0 &&
                NextChar(text, idxStart) == '{')
            {
                int idxEnd = text.IndexOf('}', idxStart);
                if(idxEnd >= 0)
                {
                    var tokenParts = GetTextParts( RemoveTags( text.Substring(idxStart, idxEnd - idxStart)).Substring(2)).ToArray();
                    if(tokenParts.Length > 0 && ( tokenType == null || tokenType.Equals(tokenParts.First(), StringComparison.OrdinalIgnoreCase)))
                    {
                        // word will default add space after "."
                        int numPartsInToken = 0;
                        while (numPartsInToken < tokenParts.Length && tokenParts[numPartsInToken++].EndsWith("."));

                        return new TokenDescription
                        {
                            StartPosition = idxStart,
                            EndPosition = idxEnd+1,
                            TokenName = tokenParts.Take(numPartsInToken).SelectMany( p => p.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries)).ToArray(),
                            Properties = tokenParts
                                    .Skip(numPartsInToken)
                                    .Select(t => {
                                            var i = t.IndexOf('=');
                                            return i > 0
                                                ? new KeyValuePair<string, string>(RemoveQuoteChars(t.Substring(0, i)), RemoveQuoteChars( t.Substring(i + 1)))
                                                : new KeyValuePair<string, string>(RemoveQuoteChars(t), string.Empty);
                                        })
                                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                        };
                    }
                }
                beginPosition = idxStart + 1;
            }
            return new TokenDescription();
        }

        public string RemoveQuoteChars(string text)
        {
            if (text.Length >= 2)
            {
                if ("‘’'“”\"«»".Contains(text[0]))
                {
                    text = text.Substring(1);
                }
                if ("‘’'“”\"«»".Contains(text.Last()))
                {
                    text = text.Substring(0, text.Length-1);
                }
            }
            return text;
        }
        private string RemoveTags(string text)
        {
            int idxStart,
                idxEnd;
            while( (idxStart = text.IndexOf( '<')) >=0 &&
                (idxEnd = text.IndexOf( '>', idxStart)) > 0) 
            {
                text = string.Concat(
                    idxStart > 0 ? text.Substring(0, idxStart) : string.Empty,
                    text.Substring(idxEnd+1));
            }
            return text;
        }

        private char NextChar(string text, int index)
        {
            while( ++index < text.Length && text[index] == '<')
            {
                if( (index = text.IndexOf('>', index)) < 0)
                {
                    return '\0';
                }
            }
            return index < text.Length ? text[index] : '\0';
        }

        private IEnumerable<string> GetTextParts( string tokenText)
        {
            var beginIdx = 0;
            for( int i=0; i<tokenText.Length; i++)
            {
                var ch = tokenText[i];
                if ( char.IsWhiteSpace(ch))
                {
                    if(i > beginIdx)
                    {
                        yield return tokenText.Substring( beginIdx, i - beginIdx);
                    }
                    beginIdx = i + 1;
                } else
                if("‘’'".Contains( ch) )
                {
                    while (++i < tokenText.Length && !"‘’'".Contains(tokenText[i])) ;
                }
                else
                if ("“”\"«»".Contains(ch))
                {
                    while (++i < tokenText.Length && !"“”\"«»".Contains(tokenText[i])) ;
                }
            }

            var lastPart = beginIdx < tokenText.Length ? tokenText.Substring(beginIdx).Trim() : null;
            if ( ! string.IsNullOrWhiteSpace( lastPart))
            {
                yield return lastPart;
            }
        }


        private static CultureInfo cultureinfo = new CultureInfo("nb-NO");
        protected string ReplaceToken(string content, TokenDescription token, object data)
        {
            if (data == null)
            {
                return string.Concat(
                    token.StartPosition > 0 ? content.Substring(0, token.StartPosition) : string.Empty,
                    "[null]",
                    content.Substring(token.EndPosition));
            }
            else if( data is string)
            {
                return ReplaceToken( content,  token, (string)data);
            }
            else if (data is decimal)
            {
                return ReplaceToken(content, token, data.ToString());
            }
            else if (data is int)
            {
                return ReplaceToken(content, token, data.ToString());
            }
            else if (data is long)
            {
                return ReplaceToken(content, token, data.ToString());
            }
            else if (data is DateTime)
            {
                var format = token.Properties.GetValue("format");
                return ReplaceToken(content, token, string.IsNullOrWhiteSpace(format) ? ((DateTime)data).ToString( "yyyy.MM.dd HH:mm") : ((DateTime)data).ToString(format, cultureinfo));
            }
            else if (data is IEnumerable)
            {
                //return ReplaceToken(content, token, (IEnumerable)data);
            }

            return string.Concat(
                token.StartPosition > 0 ? content.Substring(0, token.StartPosition) : string.Empty,
                $"[Cannot present {data.GetType().Name}]",
                content.Substring(token.EndPosition));
        }

        protected string ReplaceToken(string content, TokenDescription token, string data)
        {
            return string.Concat(
                token.StartPosition > 0 ? content.Substring(0, token.StartPosition) : string.Empty,
                data,
                content.Substring(token.EndPosition));
        }

        protected string ReplaceToken(string content, TokenDescription token, ImageFile data)
        {
            return string.Concat(
                token.StartPosition > 0 ? content.Substring(0, token.StartPosition) : string.Empty,
                _imageMarkupTemplate,
                content.Substring(token.EndPosition));
        }
        protected string ReplaceToken(string content, TokenDescription token, IEnumerable data)
        {
            // verify that there is an ListParagraph style
            if( ! StylesPart.Content.Contains("styleId=\"ListParagraph\"")) {
                StylesPart.Content = StylesPart.Content.Replace("</w:styles>", _listParagraphStyle+"</w:styles>");
            }

            // Check numbering file
            if( NumberingPart == null)
            {
                NumberingPart = new PackagePartDocument {
                    CompressionOption = CompressionOption.SuperFast,
                    Content = _numberingsFile,
                    ContentType = StylesPart.ContentType,
                    PartUri = new Uri( "/word/numbering.xml", UriKind.Relative)
                };
                var pkgList = pkgParts.ToList();
                pkgList.Add(NumberingPart);
                pkgParts = pkgList.ToArray();
                ContentPart.AddRelationships(
                    "http://schemas.openxmlformats.org/officeDocument/2006/relationships/numbering",
                    new Uri("numbering.xml", UriKind.Relative));
            }
            List<string> newValues = new List<string>();
            foreach( var item in data)
            {
                newValues.Add(_listStartMarkup);
                newValues.Add(string.Concat( 
                    "<w:r><w:t>", 
                    (item ?? string.Empty).ToString(),
                    "</w:t></w:r>"));
                newValues.Add(_listendMarkup);
            }

            return ReplaceToken(content, token, string.Concat(newValues));
        }

        protected struct TokenDescription
        {
            public int StartPosition;
            public int EndPosition;

            public string[] TokenName;
            public Dictionary<string, string> Properties;
        }

        public async Task SwapImage(string[] tokenName, Dictionary<string,string> parameters)
        {
            string originalFilename = parameters
                .Where( p => p.Key.StartsWith( "org", StringComparison.OrdinalIgnoreCase))
                .Select( p => p.Value)
                .FirstOrDefault() ?? "missing.jpg";

            string newFileName = parameters
                .Where(p => p.Key.StartsWith("org", StringComparison.OrdinalIgnoreCase))
                .Select(p => p.Value)
                .FirstOrDefault() ?? "missing.jpg";


            // Simple swap since I do not have that much time before the demo
            var firstImagePart = pkgParts.OfType<PackagePartImage>().FirstOrDefault(p => p.PartUri.ToString().StartsWith( "/word/media"));
            var catImage = (await ServiceContext.GetInfo(new[] { "cat" }, null)) as ImageFile ;
            if(firstImagePart != null && catImage != null)
            {
                firstImagePart.ContentType = catImage.ContentType;
                firstImagePart.Contents = catImage.Content;
                ContentPart.Content = ContentPart.Content.Replace(originalFilename, newFileName);
            }
        }


        private static readonly string _imageMarkupTemplate = @"
                </w:t>
            </w:r>
			<w:r>
				<w:rPr>
					<w:noProof/>
				</w:rPr>
				<w:drawing>
					<wp:inline distT=""0"" distB=""0"" distL=""0"" distR=""0"">
						<wp:extent cx=""304800"" cy=""304800""/>
						<wp:effectExtent l=""0"" t=""0"" r=""0"" b=""0""/>
						<wp:docPr id=""1"" name=""Picture 1""/>
						<wp:cNvGraphicFramePr>
							<a:graphicFrameLocks xmlns:a=""http://schemas.openxmlformats.org/drawingml/2006/main"" noChangeAspect=""1""/>
						</wp:cNvGraphicFramePr>
						<a:graphic xmlns:a=""http://schemas.openxmlformats.org/drawingml/2006/main"">
							<a:graphicData uri=""http://schemas.openxmlformats.org/drawingml/2006/picture"">
								<pic:pic xmlns:pic=""http://schemas.openxmlformats.org/drawingml/2006/picture"">
									<pic:nvPicPr>
										<pic:cNvPr id=""1"" name=""small-fun-farkle.gif""/>
										<pic:cNvPicPr/>
									</pic:nvPicPr>
									<pic:blipFill>
										<a:blip r:embed=""rId4"">
											<a:extLst>
												<a:ext uri=""{28A0092B-C50C-407E-A947-70E740481C1C}"">
													<a14:useLocalDpi xmlns:a14=""http://schemas.microsoft.com/office/drawing/2010/main"" val=""0""/>
												</a:ext>
											</a:extLst>
										</a:blip>
										<a:stretch>
											<a:fillRect/>
										</a:stretch>
									</pic:blipFill>
									<pic:spPr>
										<a:xfrm>
											<a:off x=""0"" y=""0""/>
											<a:ext cx=""304800"" cy=""304800""/>
										</a:xfrm>
										<a:prstGeom prst=""rect"">
											<a:avLst/>
										</a:prstGeom>
									</pic:spPr>
								</pic:pic>
							</a:graphicData>
						</a:graphic>
					</wp:inline>
				</w:drawing>
			</w:r>
            <w:r>
                <w:t>
            ";

        private static string _listParagraphStyle = @"
        <w:style w:type=""paragraph"" w:styleId=""ListParagraph"">
		    <w:name w:val=""List Paragraph""/>
		    <w:basedOn w:val=""Normal""/>
		    <w:uiPriority w:val=""34""/>
		    <w:qFormat/>
		    <w:pPr>
			    <w:ind w:left=""720""/>
			    <w:contextualSpacing/>
		    </w:pPr>
	    </w:style>";

        private static string _listStartMarkup = @"<w:p>
			<w:pPr>
				<w:pStyle w:val=""ListParagraph""/>
				<w:numPr>
					<w:ilvl w:val=""0""/>
					<w:numId w:val=""1""/>
				</w:numPr>
			</w:pPr>";
        private static string _listendMarkup = @"</w:p>";
        private static string _numberingsFile = @"
<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
<w:numbering xmlns:wpc=""http://schemas.microsoft.com/office/word/2010/wordprocessingCanvas"" xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006"" xmlns:o=""urn:schemas-microsoft-com:office:office"" xmlns:r=""http://schemas.openxmlformats.org/officeDocument/2006/relationships"" xmlns:m=""http://schemas.openxmlformats.org/officeDocument/2006/math"" xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:wp14=""http://schemas.microsoft.com/office/word/2010/wordprocessingDrawing"" xmlns:wp=""http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing"" xmlns:w10=""urn:schemas-microsoft-com:office:word"" xmlns:w=""http://schemas.openxmlformats.org/wordprocessingml/2006/main"" xmlns:w14=""http://schemas.microsoft.com/office/word/2010/wordml"" xmlns:w15=""http://schemas.microsoft.com/office/word/2012/wordml"" xmlns:wpg=""http://schemas.microsoft.com/office/word/2010/wordprocessingGroup"" xmlns:wpi=""http://schemas.microsoft.com/office/word/2010/wordprocessingInk"" xmlns:wne=""http://schemas.microsoft.com/office/word/2006/wordml"" xmlns:wps=""http://schemas.microsoft.com/office/word/2010/wordprocessingShape"" mc:Ignorable=""w14 w15 wp14"">
	<w:abstractNum w:abstractNumId=""0"" w15:restartNumberingAfterBreak=""0"">
		<w:nsid w:val=""2DD452FE""/>
		<w:multiLevelType w:val=""hybridMultilevel""/>
		<w:tmpl w:val=""2DAEDF9A""/>
		<w:lvl w:ilvl=""0"" w:tplc=""04090001"">
			<w:start w:val=""1""/>
			<w:numFmt w:val=""bullet""/>
			<w:lvlText w:val=""""/>
			<w:lvlJc w:val=""left""/>
			<w:pPr>
				<w:ind w:left=""720"" w:hanging=""360""/>
			</w:pPr>
			<w:rPr>
				<w:rFonts w:ascii=""Symbol"" w:hAnsi=""Symbol"" w:hint=""default""/>
			</w:rPr>
		</w:lvl>
		<w:lvl w:ilvl=""1"" w:tplc=""04090003"" w:tentative=""1"">
			<w:start w:val=""1""/>
			<w:numFmt w:val=""bullet""/>
			<w:lvlText w:val=""o""/>
			<w:lvlJc w:val=""left""/>
			<w:pPr>
				<w:ind w:left=""1440"" w:hanging=""360""/>
			</w:pPr>
			<w:rPr>
				<w:rFonts w:ascii=""Courier New"" w:hAnsi=""Courier New"" w:cs=""Courier New"" w:hint=""default""/>
			</w:rPr>
		</w:lvl>
		<w:lvl w:ilvl=""2"" w:tplc=""04090005"" w:tentative=""1"">
			<w:start w:val=""1""/>
			<w:numFmt w:val=""bullet""/>
			<w:lvlText w:val=""""/>
			<w:lvlJc w:val=""left""/>
			<w:pPr>
				<w:ind w:left=""2160"" w:hanging=""360""/>
			</w:pPr>
			<w:rPr>
				<w:rFonts w:ascii=""Wingdings"" w:hAnsi=""Wingdings"" w:hint=""default""/>
			</w:rPr>
		</w:lvl>
		<w:lvl w:ilvl=""3"" w:tplc=""04090001"" w:tentative=""1"">
			<w:start w:val=""1""/>
			<w:numFmt w:val=""bullet""/>
			<w:lvlText w:val=""""/>
			<w:lvlJc w:val=""left""/>
			<w:pPr>
				<w:ind w:left=""2880"" w:hanging=""360""/>
			</w:pPr>
			<w:rPr>
				<w:rFonts w:ascii=""Symbol"" w:hAnsi=""Symbol"" w:hint=""default""/>
			</w:rPr>
		</w:lvl>
		<w:lvl w:ilvl=""4"" w:tplc=""04090003"" w:tentative=""1"">
			<w:start w:val=""1""/>
			<w:numFmt w:val=""bullet""/>
			<w:lvlText w:val=""o""/>
			<w:lvlJc w:val=""left""/>
			<w:pPr>
				<w:ind w:left=""3600"" w:hanging=""360""/>
			</w:pPr>
			<w:rPr>
				<w:rFonts w:ascii=""Courier New"" w:hAnsi=""Courier New"" w:cs=""Courier New"" w:hint=""default""/>
			</w:rPr>
		</w:lvl>
		<w:lvl w:ilvl=""5"" w:tplc=""04090005"" w:tentative=""1"">
			<w:start w:val=""1""/>
			<w:numFmt w:val=""bullet""/>
			<w:lvlText w:val=""""/>
			<w:lvlJc w:val=""left""/>
			<w:pPr>
				<w:ind w:left=""4320"" w:hanging=""360""/>
			</w:pPr>
			<w:rPr>
				<w:rFonts w:ascii=""Wingdings"" w:hAnsi=""Wingdings"" w:hint=""default""/>
			</w:rPr>
		</w:lvl>
		<w:lvl w:ilvl=""6"" w:tplc=""04090001"" w:tentative=""1"">
			<w:start w:val=""1""/>
			<w:numFmt w:val=""bullet""/>
			<w:lvlText w:val=""""/>
			<w:lvlJc w:val=""left""/>
			<w:pPr>
				<w:ind w:left=""5040"" w:hanging=""360""/>
			</w:pPr>
			<w:rPr>
				<w:rFonts w:ascii=""Symbol"" w:hAnsi=""Symbol"" w:hint=""default""/>
			</w:rPr>
		</w:lvl>
		<w:lvl w:ilvl=""7"" w:tplc=""04090003"" w:tentative=""1"">
			<w:start w:val=""1""/>
			<w:numFmt w:val=""bullet""/>
			<w:lvlText w:val=""o""/>
			<w:lvlJc w:val=""left""/>
			<w:pPr>
				<w:ind w:left=""5760"" w:hanging=""360""/>
			</w:pPr>
			<w:rPr>
				<w:rFonts w:ascii=""Courier New"" w:hAnsi=""Courier New"" w:cs=""Courier New"" w:hint=""default""/>
			</w:rPr>
		</w:lvl>
		<w:lvl w:ilvl=""8"" w:tplc=""04090005"" w:tentative=""1"">
			<w:start w:val=""1""/>
			<w:numFmt w:val=""bullet""/>
			<w:lvlText w:val=""""/>
			<w:lvlJc w:val=""left""/>
			<w:pPr>
				<w:ind w:left=""6480"" w:hanging=""360""/>
			</w:pPr>
			<w:rPr>
				<w:rFonts w:ascii=""Wingdings"" w:hAnsi=""Wingdings"" w:hint=""default""/>
			</w:rPr>
		</w:lvl>
	</w:abstractNum>
	<w:num w:numId=""1"">
		<w:abstractNumId w:val=""0""/>
	</w:num>
</w:numbering>
";
    }

}